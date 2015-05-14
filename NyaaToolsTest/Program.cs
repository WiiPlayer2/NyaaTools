using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NyaaTools;

namespace NyaaToolsTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            NyaaPageListener plistener = new NyaaPageListener("76430", "511065", new TimeSpan(0, 1, 0));
            plistener.NewTorrentReceived += plistener_NewTorrentReceived;
            plistener.Start();
            Console.ReadKey(true);
            plistener.Stop();
        }

        private static void plistener_NewTorrentReceived(NyaaTorrent pTorrent)
        {
            Console.WriteLine("[{0}] - {1}", pTorrent.TorrentID, pTorrent.Name);
        }
    }
}