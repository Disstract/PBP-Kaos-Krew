using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text.Json;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Security.Cryptography.X509Certificates;

namespace KaosPrefetch
{
    public class ProgramClass
    {
        public static void Main(string[] args)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            long starttime = now.ToUnixTimeMilliseconds();

            List<string> Games = new List<string>();
            List<string> Links = new List<string>();

            PrefetchCacher.GetAllPostsFromPage();

            Games = PrefetchCacher.GameTitles;
            Links = PrefetchCacher.ForumLinks;

            List<FetchQuery> Fetchqueries = new List<FetchQuery>();


            for(int i = 0; i < Games.Count; i++)
            {
                FetchQuery query = new FetchQuery();
                query.Title = Games[i];
                query.URL = Links[i];

                Fetchqueries.Add(query);
            }

            QueryList response = new QueryList();
            response.response = Fetchqueries;

            string Outputpath = args[0];

            string jsonString = JsonSerializer.Serialize<QueryList>(response);
            File.WriteAllText(Outputpath, jsonString);

            Console.WriteLine(string.Format("Amount of Titles Retrieved: {0}, Amount of Links Retrieved: {1}", Games.Count, Links.Count));

            DateTimeOffset endnow = DateTimeOffset.UtcNow;
            long endtime = endnow.ToUnixTimeMilliseconds();

            float runtime = (endtime - starttime) / 1000;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(runtime);
        }
    }


    public class PrefetchCacher
    {
        public static List<string> ForumLinks = new List<string>();
        public static List<string> GameTitles = new List<string>();

        public static string URL = "https://kaoskrew.org/viewforum.php?f=13&start=" + LinksMount.ToString();
        public static int LinksMount = 0;

        public static int AmountofPages = 0;
        public static int CurrentPage = 0;       

        public static void GetAllPostsFromPage()
        {
            List<string> Links = new List<string>();
            List<string> Titles = new List<string>();

            var options = new ChromeOptions()
            {
                BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            };

            options.AddArguments(new List<string>() { "headless", "disable-gpu" });
            var browser = new ChromeDriver(options);           
            
            if(AmountofPages == 0)
            {
                browser.Navigate().GoToUrl(URL);
                var FinalPages = browser.FindElements(By.ClassName("button"));
                List<string> StuffList = new List<string>();

                foreach (var Stuff in FinalPages)
                {
                    StuffList.Add(Stuff.GetAttribute("text"));
                }

                AmountofPages = Convert.ToInt32(StuffList[8]);
            }
            
            while(CurrentPage < AmountofPages)
            {
                //Goes to the forum page
                browser.Navigate().GoToUrl(URL);

                //Grabs all the games and appends them to Links and Titles
                var links = browser.FindElements(By.ClassName("topictitle"));

                Regex VersionsSplitLower = new Regex("v[0-9]+");
                Regex VersionsSplitUpper = new Regex("V[0-9]+");
                Regex VersionsSplitSpacedLower = new Regex("v [0-9]+");
                Regex VersionsSplitSpacedUpper = new Regex("V [0-9]+");

                Regex BuildType1 = new Regex("Build [0-9]+");
                Regex BuildType2 = new Regex("Build[0-9]+");
                Regex BuildType3 = new Regex("build [0-9]+");

                foreach (var a in links)
                {
                    var e = (a.GetAttribute("href"));
                    var title = a.GetAttribute("text");

                    string T1 = title.Replace(".", " ");
                    string[] T2 = T1.Split(new[] { "Multi", "multi", "MULTi", "MULTI" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] T3 = VersionsSplitLower.Split(T2[0]);
                    string[] T4 = VersionsSplitUpper.Split(T3[0]);
                    string[] T5 = T4[0].Split(new[] { "Update", "REPACK-KaOs", "REPACK2-KaOs", "REPACK" }, StringSplitOptions.RemoveEmptyEntries);
                    string T6 = T5[0].Replace("[", "").TrimEnd();

                    string[] T7 = BuildType1.Split(T6);
                    string[] T8 = BuildType2.Split(T7[0]);
                    string[] T9 = BuildType3.Split(T8[0]);

                    string[] T10 = VersionsSplitSpacedLower.Split(T9[0]);
                    string[] T11 = VersionsSplitSpacedUpper.Split(T10[0]);

                    if(T11[0].TrimEnd() != "KaOs Krew Game Release Index A-Z]")
                    {
                        GameTitles.Add(T11[0]);
                        ForumLinks.Add(e);
                    }
                }               

                //Once all the games are grabbed, gets the next page URL
                Console.WriteLine(string.Format("Amount of pages {0}, Current Page {1}, Page Link {2}", AmountofPages.ToString(), CurrentPage.ToString(), URL));
                CurrentPage++;

                LinksMount += 50;
                URL = "https://kaoskrew.org/viewforum.php?f=13&start=" + LinksMount.ToString();
            }

            browser.Quit();
        }
    }
}