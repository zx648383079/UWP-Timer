using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.LogTimer.Models;

namespace ZoDream.LogTimer.ViewModels
{
    public class ArticleDetailViewModel: BindableBase
    {

        private Article article = new() {
            User = new User(),
            Term = new ArticleCategory()
        };

        public Article Article
        {
            get => article;
            set => Set(ref article, value);
        }

    }
}
