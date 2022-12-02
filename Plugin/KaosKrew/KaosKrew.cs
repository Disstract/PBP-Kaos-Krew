using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaosKrew
{
    internal class KaosKrew : Project_Black_Pearl.SDK.PBPPlugin
    {
        public string Title => "KaosKrew";
        public string Type => "Binary";


        public bool isPrefetch => true;
        public string FirstPayloadScraper => Path.Combine(InfoPath.PBPExtensions, "KaosPrecache\\KaosPrecache.exe"); 
        public string FirstPayloadType => "Binary";


        public string ScraperPath => Path.Combine(InfoPath.PBPExtensions, "KaosPostScrape\\KaosPostScrape.exe");    


        public string URL1Image => InfoPath.PBPImages + "URL1.png";
        public string URL1Type => "";
        public string URL1Validator => "";


        public string URL2Image => InfoPath.PBPImages + "URL2.png";
        public string URL2Type => "";
        public string URL2Validator => "";


        public string URL3Image => InfoPath.PBPImages + "URL3.png";
        public string URL3Type => "";
        public string URL3Validator => "";


        public string URL4Image => InfoPath.PBPImages + "URL4.png";
        public string URL4Type => "";
        public string URL4Validator => "";
    }

    public class InfoPath
    {
        public static string systempath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string PBPExtensions = Path.Combine(systempath, "Project Black Pearl\\Extensions\\Scrapers");
        public static string PBPImages = Path.Combine(systempath, "Project Black Pearl\\Extensions\\Images\\");
    }
}
