using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWP_Timer.Controls
{
    [TemplatePart(Name = "Canvas", Type = typeof(CanvasControl))]
    public sealed class Chart : Control
    {
        public Chart()
        {
            this.DefaultStyleKey = typeof(Chart);
            Loaded += Chart_Loaded;
            Unloaded += Chart_Unloaded;
        }

        private void Chart_Unloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void Chart_Loaded(object sender, RoutedEventArgs e)
        {
            var canvas = GetTemplateChild("Canvas") as CanvasControl;
            if (canvas == null)
            {
                return;
            }
            canvas.Draw += Canvas_Draw;
            canvas.Invalidate();
        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void RefreshView()
        {
            (GetTemplateChild("Canvas") as CanvasControl).Invalidate();
        }

        public void Dispose()
        {
            var canvas = GetTemplateChild("Canvas") as CanvasControl;
            if (canvas != null)
            {
                canvas.RemoveFromVisualTree();
            }
        }
    }
}
