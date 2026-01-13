using System.Collections.Generic;

namespace ZoDream.LogTimer.Repositories.Models
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
        public int Topic { get; set; }


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
            if (Topic > 0)
            {
                data.Add("topic", Topic.ToString());
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

    public class Queries
    {
        public string Keywords { get; set; } = "";
        public uint Page { get; set; } = 1;
        public uint PerPage { get; set; } = 20;
    }

    public class TaskQueries : Queries
    {
        public int User { get; set; }
        public int Status { get; set; }
    }

    public class MicroQueries : Queries
    {
        public int User { get; set; }
        public int Topic { get; set; }

        public int Status { get; set; }
    }

    public class MicroCommentQueries : Queries
    {
        public int Id { get; set; }
    }
}
