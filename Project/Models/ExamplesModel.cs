using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class ExamplesModel: HomeModel
    {
            public string Name { get; set; }

            public ExamplesItem[] Items { get; set; }

            public ExamplesModel(MenuModel menu, string name, IEnumerable<ExamplesItem> items) : base(menu)
            {
                Name = name;
                Items = items.ToArray();
            }

            public class ExamplesItem
            {
                public string Name { get; set; }

                public string Link { get; set; }

                public ExamplesItem(string name, string link)
                {
                    Name = name;
                    Link = link;
                }
            }
        }
    }
