using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;

namespace ZoDream.LogTimer.Repositories.Models
{
    public class CheckInBatch
    {
        public CheckIn Today { get; set; }
        public IList<CheckIn> Month { get; set; }
    }

    public class BatchData
    {
        public AppOption SeoConfigs { get; set; }

        public User AuthProfile { get; set; }
    }
}
