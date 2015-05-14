using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace NyaaTools
{
    public class NyaaUserPage : NyaaPage
    {
        public int SubmitterID { get; internal set; }

        public string SubmitterName { get; internal set; }

        public NyaaFilter Filter { get; internal set; }

        public override void UpdateData()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(string.Format("http://{0}.nyaa.se/?user={1}&offset={2}" , NyaaModeHelper.GetSubDomain(Mode), SubmitterID, Page));
                var node = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[3]/span/a/span");
                SubmitterName = node.InnerText;
                HtmlNode table = doc.DocumentNode.ChildNodes["html"].ChildNodes["body"].ChildNodes[1].ChildNodes[2].ChildNodes[8];
                UpdateFromTable(table);
            }
            catch (Exception e)
            {
                CallInternalException(e);
            }
        }

        protected override void UpdateSpecial(HtmlNode row, NyaaTorrent torrent)
        {
            torrent.SubmitterID = SubmitterID;
        }

        public override string ToString()
        {
            return string.Format("{0} - [{1}]", SubmitterName, SubmitterID);
        }
    }
}
