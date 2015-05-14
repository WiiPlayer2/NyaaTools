using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NyaaTools
{
    public class NyaaSearchPage : NyaaPage
    {
        public string CategoryName { get; internal set; }

        public NyaaFilter Filter { get; internal set; }

        public string SearchQuery { get; internal set; }

        public int? UserId { get; internal set; }

        public NyaaSorting Sorting{get;internal set;}

        public NyaaSortingOrder SortingOrder { get; internal set; }

        private string GenerateQuery()
        {
            var tmp = new List<string>();

            if (CategoryName != null)
            {
                tmp.Add(string.Format("cats={0}", NyaaModeHelper.GetCategoryId(CategoryName, Mode)));
            }
            if (SearchQuery != null)
            {
                tmp.Add(string.Format("term={0}", Uri.EscapeDataString(SearchQuery)));
            }
            if(UserId.HasValue)
            {
                tmp.Add(string.Format("user={0}",UserId.Value));
            }
            tmp.Add(string.Format("filter={0}", NyaaModeHelper.GetFilterId(Filter)));
            tmp.Add(string.Format("offset={0}", Page));
            tmp.Add(string.Format("sort={0}", (int)Sorting));
            tmp.Add(string.Format("order={0}",(int)SortingOrder));

            return string.Join("&", tmp.ToArray());
        }

        public override void UpdateData()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(string.Format("http://{0}.nyaa.se/?page=search&{1}", NyaaModeHelper.GetSubDomain(Mode), GenerateQuery()));
                HtmlNode table = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[3]/table[2]");
                UpdateFromTable(table);
            }
            catch(Exception e)
            {
                CallInternalException(e);
            }
        }

        protected override void UpdateSpecial(HtmlNode row, NyaaTorrent torrent)
        {
            if (UserId.HasValue)
            {
                torrent.SubmitterID = UserId.Value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}", SearchQuery, CategoryName, Filter, UserId, Mode);
        }
    }
}
