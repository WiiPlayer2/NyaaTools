using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace NyaaTools
{
    public class NyaaTorrent
    {
        public string Name { get; internal set; }
        public string SubmitterName { get; internal set; }
        public int SubmitterID { get; internal set; }
        public string Tracker { get; internal set; }
        public string Information { get; internal set; }
        public DateTime Date { get; internal set; }
        public int Seeders { get; internal set; }
        public int Leechers { get; internal set; }
        public int Downloads { get; internal set; }
        public int TorrentID { get; internal set; }
        public string TorrentUrl
        {
            get
            {
                return string.Format("http://{0}.nyaa.se/?page=download&tid={1}", NyaaModeHelper.GetSubDomain(Mode), TorrentID);
            }
        }
        public NyaaMode Mode { get; internal set; }

        private void UpdateData(HtmlNode table)
        {
            Name = table.ChildNodes[1].ChildNodes[1].InnerText;

            string date = table.ChildNodes[1].ChildNodes[3].InnerText;
            Date = DateTime.Parse(date.Substring(0, date.Length - 4));
            //Date = Date.ToLocalTime();

            HtmlNode link = table.ChildNodes[2].ChildNodes[1].ChildNodes[0];
            string sublink = link.GetAttributeValue("href", "-");
            SubmitterID = int.Parse(sublink.Substring(sublink.LastIndexOf('=') + 1));
            SubmitterName = link.InnerText;

            Seeders = int.Parse(table.ChildNodes[2].ChildNodes[3].InnerText);

            Tracker = table.ChildNodes[3].ChildNodes[1].InnerText;

            Leechers = int.Parse(table.ChildNodes[3].ChildNodes[3].InnerText);

            Information = table.ChildNodes[4].ChildNodes[1].InnerText;

            Downloads = int.Parse(table.ChildNodes[4].ChildNodes[3].InnerText);
        }

        public static NyaaTorrent ScanTorrent(int pId)
        {
            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load(string.Format("http://www.nyaa.se/?page=view&tid={1}", pId));
            //HtmlNode table = doc.DocumentNode.ChildNodes["html"].ChildNodes["body"].ChildNodes[1].ChildNodes[2].ChildNodes[0].ChildNodes[1];

            //NyaaTorrent ret = new NyaaTorrent();
            //ret.TorrentID = pId;
            //ret.UpdateData(table);

            //return ret;
            return ScanTorrent(pId, NyaaMode.Normal);
        }

        public static NyaaTorrent ScanTorrent(int pId, NyaaMode mode)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(string.Format("http://{0}.nyaa.se/?page=view&tid={1}", NyaaModeHelper.GetSubDomain(mode), pId));
            HtmlNode table = doc.DocumentNode.ChildNodes["html"].ChildNodes["body"].ChildNodes[1].ChildNodes[2].ChildNodes[0].ChildNodes[1];

            NyaaTorrent ret = new NyaaTorrent();
            ret.TorrentID = pId;
            ret.Mode = mode;
            ret.UpdateData(table);

            return ret;
        }
    }
}
