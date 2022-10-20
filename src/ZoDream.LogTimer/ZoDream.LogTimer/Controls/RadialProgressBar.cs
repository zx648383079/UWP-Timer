using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.LogTimer.Controls
{
    [TemplatePart(Name = "Canvas", Type = typeof(CanvasControl))]
    [TemplatePart(Name = "ValueTb", Type = typeof(TextBlock))]
    public sealed class RadialProgressBar : Control
    {
        public RadialProgressBar()
        {
            DefaultStyleKey = typeof(RadialProgressBar);
            Loaded += RadialProgressBar_Loaded;
            Unloaded += RadialProgressBar_Unloaded;
        }

        private void RadialProgressBar_Unloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void RadialProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            var canvas = GetTemplateChild("Canvas") as CanvasControl;
            if (canvas == null)
            {
                return;
            }
            canvas.Draw += Canvas_Draw;
            canvas.Invalidate();
        }

        private DateTime _startTime;
        private DispatcherTimer _timer;

        /// <summary>
        /// 最大值
        /// </summary>
        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(RadialProgressBar), new PropertyMetadata(0, new PropertyChangedCallback(valueChangedCallback)));

        /// <summary>
        /// 当前进度
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(RadialProgressBar), new PropertyMetadata(0, valueChangedCallback));


        private static void valueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RadialProgressBar).RefreshView();
        }


        /// <summary>
        /// 圆边底色
        /// </summary>
        public Color Outline
        {
            get { return (Color)GetValue(OutlineProperty); }
            set { SetValue(OutlineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Outline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutlineProperty =
            DependencyProperty.Register("Outline", typeof(Color), typeof(RadialProgressBar), new PropertyMetadata(Color.FromArgb(76, 38, 129, 215)));

        /// <summary>
        /// 圆边已完成颜色
        /// </summary>
        public Color Inline
        {
            get { return (Color)GetValue(InlineProperty); }
            set { SetValue(InlineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Inline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InlineProperty =
            DependencyProperty.Register("Inline", typeof(Color), typeof(RadialProgressBar), new PropertyMetadata(Color.FromArgb(255, 81, 235, 62)));




        public Color InlineBackground
        {
            get { return (Color)GetValue(InlineBackgroundProperty); }
            set { SetValue(InlineBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InlineBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InlineBackgroundProperty =
            DependencyProperty.Register("InlineBackground", typeof(Color), typeof(RadialProgressBar), new PropertyMetadata(Color.FromArgb(76, 255, 255, 255)));



        public int LineWidth
        {
            get { return (int)GetValue(LineWidthProperty); }
            set { SetValue(LineWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineWidthProperty =
            DependencyProperty.Register("LineWidth", typeof(int), typeof(RadialProgressBar), new PropertyMetadata(3));



        /// <summary>
        /// 进度事件
        /// </summary>
        public event RangeBaseValueChangedEventHandler ValueChanged;
        /// <summary>
        /// 进度完成最大值事件
        /// </summary>
        public event RangeBaseValueChangedEventHandler TimeEnd;

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            _startTime = DateTime.Now.AddSeconds(-Value);
            if (_timer != null)
            {
                _timer.Start();
                return;
            }
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            _timer.Tick += new EventHandler<object>((sender, e) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    var diff = (DateTime.Now - _startTime).TotalSeconds;
                    if (Max > 0 && diff >= Max * 60)
                    {
                        Stop();
                        return;
                    }
                    Value = Convert.ToInt32(diff);
                    ValueChanged?.Invoke(this, null);
                });
            });
            _timer.Start();
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            if (_timer == null)
            {
                return;
            }
            _timer.Stop();
            TimeEnd?.Invoke(this, null);
        }

        public double Progress
        {
            get
            {
                if (Max > 0)
                {
                    return (double)Value * 100 / (Max * 60);
                }
                else
                {
                    return 0;
                }
            }
        }

        public void RefreshView()
        {
            (GetTemplateChild("Canvas") as CanvasControl).Invalidate();
        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var progress = Progress;
            var centerX = (float)ActualWidth / 2;
            var centerY = (float)ActualHeight / 2;
            var radius = Math.Min(centerX, centerY);
            var lineRadius = radius - LineWidth;
            var inlineRadius = lineRadius - 5;
            using (var draw = args.DrawingSession)
            {
                draw.FillRectangle(new Windows.Foundation.Rect(centerX - radius, centerY - radius, 2 * radius, 2 * radius),
                    Colors.Transparent);
                draw.FillCircle(centerX, centerY, inlineRadius, InlineBackground);
                draw.DrawCircle(centerX, centerY, lineRadius, Outline, LineWidth);
                var deg = (2 * Math.PI / 100 * progress)
                    ; // 圆环的绘制
                draw.DrawGeometry(Arc(draw, centerX, centerY, lineRadius, (float)(-.5 * Math.PI), (float)deg), Inline, LineWidth);

                var x = (float)(centerX + Math.Cos(Math.PI * 2 * (progress - 25) / 100) * lineRadius);
                var y = (float)(centerY + Math.Sin(Math.PI * 2 * (progress - 25) / 100) * lineRadius);
                draw.FillCircle(x, y, LineWidth, Inline);
            }
        }

        public CanvasGeometry Arc(ICanvasResourceCreator resourceCreator, float centerX, float centerY, float radius, float startAngle, float endAngle)
        {
            var path = new CanvasPathBuilder(resourceCreator);
            path.BeginFigure(centerX, centerY - radius);
            path.AddArc(new Vector2(centerX, centerY), radius, radius, startAngle, endAngle);
            path.EndFigure(CanvasFigureLoop.Open);
            return CanvasGeometry.CreatePath(path);
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
            var canvas = GetTemplateChild("Canvas") as CanvasControl;
            if (canvas != null)
            {
                canvas.RemoveFromVisualTree();
            }
        }
    }
}
