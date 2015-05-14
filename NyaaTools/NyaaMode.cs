using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NyaaTools
{
    public enum NyaaMode
    {
        Normal,
        Sukebei
    }

    public static class NyaaModeHelper
    {
        private static Dictionary<string, string> normalCategories;
        private static Dictionary<string, string> sukebeiCategories;

        static NyaaModeHelper()
        {
            normalCategories = new Dictionary<string, string>()
            {
                {"All categories", "0_0"},
                {"Anime", "1_0"},
                {"Anime - Anime Music Video", "1_32"},
                {"Anime - English-translated Anime", "1_37"},
                {"Anime - Non-English-translated Anime", "1_38"},
                {"Anime - Raw Anime", "1_11"},
                {"Audio", "3_0"},
                {"Audio - Lossless Audio", "3_14"},
                {"Audio - Lossy Audio", "3_15"},
                {"Books","2_0"},
                {"Books - English-scanlated Books", "2_12"},
                {"Books - Non-English-scanlated Books", "2_39"},
                {"Books - Raw Books", "2_13"},
                {"Live Action", "5_0"},
                {"Live Action - English-translated Live Action", "5_19"},
                {"Live Action - Live Action Promotional Video", "5_22"},
                {"Live Action - Non-English-translated Live Action", "5_21"},
                {"Pictures", "4_0"},
                {"Pictures - Graphics", "4_18"},
                {"Pictures - Photos", "4_17"},
                {"Software", "6_0"},
                {"Software - Applications", "6_23"},
                {"Software - Games", "6_24"}
            };

            sukebeiCategories = new Dictionary<string, string>()
            {
                {"All categories", "0_0"},
                {"Art", "7_0"},
                {"Art - Anime", "7_25"},
                {"Art - Doujinshi", "7_33"},
                {"Art - Games", "7_27"},
                {"Art - Manga", "7_26"},
                {"Art - Pictures", "7_28"},
                {"Real Life", "8_0"},
                {"Real Life - Photobooks & Pictures", "8_31"},
                {"Real Life - Videos", "8_30"}
            };
        }

        internal static string GetSubDomain(NyaaMode mode)
        {
            string ret = null;
            switch(mode)
            {
                case NyaaMode.Normal:
                    ret = "www";
                    break;
                case NyaaMode.Sukebei:
                    ret = "sukebei";
                    break;
            }
            return ret;
        }

        public static string[] GetCategories(NyaaMode mode)
        {
            string[] ret = null;

            switch(mode)
            {
                case NyaaMode.Normal:
                    ret = normalCategories.Keys.ToArray();
                    break;
                case NyaaMode.Sukebei:
                    ret = sukebeiCategories.Keys.ToArray();
                    break;
            }

            return ret;
        }

        public static string GetCategoryId(string name, NyaaMode mode)
        {
            string ret = null;

            switch(mode)
            {
                case NyaaMode.Normal:
                    ret = normalCategories[name];
                    break;
                case NyaaMode.Sukebei:
                    ret = sukebeiCategories[name];
                    break;
            }

            return ret;
        }

        public static string GetFilterId(NyaaFilter filter)
        {
            return ((int)filter).ToString();
        }
    }
}
