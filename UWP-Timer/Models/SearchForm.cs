using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 搜索表单
    /// </summary>
    public class SearchForm
    {
        public int Category { get; set; }
        public string Keywords { get; set; }
        public int Page { get; set; }
        public uint PerPage { get; set; } = 20;
        public int Status { get; set; }

        public int User { get; set; }


        public string Sort { get; set; }

        public string Type { get; set; }
        public string Date { get; set; }

        public Dictionary<string, string> ToQueries()
        {
            var data = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(Keywords))
            {
                data.Add("keywords", Keywords);
            }
            if (!string.IsNullOrWhiteSpace(Type))
            {
                data.Add("type", Type);
            }
            if (!string.IsNullOrWhiteSpace(Sort))
            {
                data.Add("sort", Sort);
            }
            if (!string.IsNullOrWhiteSpace(Date))
            {
                data.Add("date", Date);
            }
            if (Category > 0)
            {
                data.Add("category", Category.ToString());
            }
            if (User != 0)
            {
                data.Add("user", User.ToString());
            }
            if (Status > 0)
            {
                data.Add("status", Status.ToString());
            }
            if (Page < 1)
            {
                Page = 1;
            }
            if (PerPage < 1)
            {
                PerPage = 20;
            }
            data.Add("page", Page.ToString());
            data.Add("per_page", PerPage.ToString());
            return data;
        }
    }
}
