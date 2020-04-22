using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Models;

namespace UWP_Timer.ViewModels
{
    public class ArticleDetailViewModel: BindableBase
    {

        private Article article = new Article() {
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
