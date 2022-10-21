﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Loggers
{
    public interface ILogger
    {
        public LogLevel Level { get; }

        public void Log(LogLevel level, string message);
        public void Log(string message);

        public void Info(string message);

        public void Waining(string message);

        public void Error(string message);

        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="current"></param>
        /// <param name="total"></param>
        public void Progress(long current, long total);
    }
}
