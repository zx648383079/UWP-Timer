﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 注销表单
    /// </summary>
    public class CancelForm
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
