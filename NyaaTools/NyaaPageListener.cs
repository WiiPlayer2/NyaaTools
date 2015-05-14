using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Timers;

namespace NyaaTools
{
    public class NyaaPageListener
    {
        //private NyaaPage page;
        private TimeSpan refreshTime;
        private bool running;
        private Timer timer;

        public NyaaPageListener(NyaaPage pPage, TimeSpan pRefreshTime)
            : this(pPage, 0, pRefreshTime)
        {
        }

        public NyaaPageListener(int pUserId, TimeSpan pRefreshTime)
            : this(pUserId, 0, pRefreshTime)
        {
        }

        public NyaaPageListener(NyaaPage pPage, int pLastId, TimeSpan pRefreshTime)
        {
            Page = pPage;
            LastTorrentID = pLastId;
            refreshTime = pRefreshTime;

            timer = new Timer(refreshTime.TotalMilliseconds) {AutoReset = true};
            timer.Elapsed += timer_Elapsed;
        }

        public NyaaPageListener(int pUserId, int pLastId, TimeSpan pRefreshTime)
        {
            Page = new NyaaUserPage();
            ((NyaaUserPage)Page).SubmitterID = pUserId;
            LastTorrentID = pLastId;
            refreshTime = pRefreshTime;

            timer = new Timer(refreshTime.TotalMilliseconds) {AutoReset = true};
            timer.Elapsed += timer_Elapsed;
        }

        public delegate void NewTorrentEventHandler(NyaaTorrent pTorrent);

        public event NewTorrentEventHandler NewTorrentReceived;

        public int LastTorrentID { get; set; }

        public NyaaPage Page { get; private set; }

        public void Start()
        {
            //Thread thread = new Thread(new ThreadStart(CheckLoop));
            //running = true;
            //thread.Start();
            timer_Elapsed(timer, null);
            timer.Start();
        }

        public void Stop()
        {
            //running = false;
            timer.Stop();
        }

        //private void CheckLoop()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    while (running)
        //    {
        //        sw.Restart();
        //        page.UpdateData();
        //        for (int i = page.Torrents.Length - 1; i >= 0; i--)
        //        {
        //            if (int.Parse(page.Torrents[i].TorrentID) > int.Parse(LastTorrentID))
        //            {
        //                LastTorrentID = page.Torrents[i].TorrentID;
        //                if (NewTorrentReceived != null)
        //                {
        //                    NewTorrentReceived(page.Torrents[i]);
        //                }
        //            }
        //        }
        //        while (running && sw.Elapsed < refreshTime)
        //        {
        //            Thread.Sleep(100);
        //        }
        //    }

        //    sw.Stop();
        //}

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Page.UpdateData();
            for (int i = Page.Torrents.Length - 1; i >= 0; i--)
            {
                if (Page.Torrents[i].TorrentID > LastTorrentID)
                {
                    LastTorrentID = Page.Torrents[i].TorrentID;
                    if (NewTorrentReceived != null)
                    {
                        NewTorrentReceived(Page.Torrents[i]);
                    }
                }
            }
        }
    }
}