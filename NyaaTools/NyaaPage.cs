using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Diagnostics;

namespace NyaaTools
{
    public abstract class NyaaPage
    {
        protected NyaaPage()
        {
            Torrents = new NyaaTorrent[0];
        }

        public NyaaTorrent[] Torrents { get; internal set; }

        public NyaaMode Mode { get; internal set; }

        public int Page { get; internal set; }

        public static NyaaUserPage ScanUser(int pId, NyaaMode mode = NyaaMode.Normal)
        {
            NyaaUserPage ret = new NyaaUserPage();
            ret.SubmitterID = pId;
            ret.Mode = mode;
            ret.Page = 1;
            ret.UpdateData();

            return ret;
        }

        public static NyaaSearchPage Search(
            string searchQuery = null,
            string categoryName = null,
            NyaaFilter filter = NyaaFilter.All,
            int? userId = null,
            NyaaSorting sorting = NyaaSorting.Date,
            NyaaSortingOrder sortingOrder = NyaaSortingOrder.Descending,
            NyaaMode mode = NyaaMode.Normal)
        {
            var page = new NyaaSearchPage();
            page.SearchQuery = searchQuery;
            page.CategoryName = categoryName;
            page.Filter = filter;
            page.UserId = userId;
            page.Mode = mode;
            page.Page = 1;
            page.Sorting = sorting;
            page.SortingOrder = sortingOrder;
            page.UpdateData();
            return page;
        }

        public abstract void UpdateData();

        protected void UpdateFromTable(HtmlNode table)
        {
            if (table.ChildNodes.Count == 2 && table.ChildNodes[1].InnerText == "No torrents found.")
            {
                Torrents = new NyaaTorrent[0];
            }
            else
            {

                NyaaTorrent[] tlist = new NyaaTorrent[table.ChildNodes.Count - 1];
                for (int i = 1; i < table.ChildNodes.Count; i++)
                {
                    NyaaTorrent t = new NyaaTorrent();

                    string link = table.ChildNodes[i].ChildNodes[1].ChildNodes[0].GetAttributeValue("href", "-");
                    t.TorrentID = int.Parse(link.Substring(link.LastIndexOf('=') + 1));

                    t.Name = table.ChildNodes[i].ChildNodes[1].InnerText;

                    int tmp;
                    t.Seeders = int.TryParse(table.ChildNodes[i].ChildNodes[4].InnerText, out tmp) ? tmp : -1;
                    t.Leechers = int.Parse(table.ChildNodes[i].ChildNodes[5].InnerText);
                    if (t.Seeders != -1)
                    {
                        t.Downloads = int.Parse(table.ChildNodes[i].ChildNodes[6].InnerText);
                    }
                    else
                    {
                        t.Downloads = t.Leechers;
                        t.Leechers = -1;
                    }
                    t.Mode = Mode;

                    UpdateSpecial(table.ChildNodes[i], t);

                    tlist[i - 1] = t;
                }
                Torrents = tlist;
            }
        }

        protected abstract void UpdateSpecial(HtmlNode row, NyaaTorrent torrent);

        public void ResetPage()
        {
            Page = 1;
            UpdateData();
        }

        public void NextPage()
        {
            Page++;
            UpdateData();
        }

        public delegate void InternalExceptionEventHandler(NyaaPage sender, Exception e);
        public static event InternalExceptionEventHandler InternalException;
        protected void CallInternalException(Exception e)
        {
            if (InternalException != null)
            {
                InternalException(this, e);
            }
        }
    }
}