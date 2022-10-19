using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.LogTimer.Models
{
    public class CheckInBatch
    {
        public CheckIn Today { get; set; }
        public IList<CheckIn> Month { get; set; }
    }
}
