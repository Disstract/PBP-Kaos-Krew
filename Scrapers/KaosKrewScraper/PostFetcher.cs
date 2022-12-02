using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace KaosKrewScraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //First arg is the URL - 0
            //Second arg is the output folder - 1
            //Third arg is the game title - 2
            //Fourth arg is the Done indicator - 3

            PostFetcher.GetLinks(args[0]);
            List<string> MultiLinks = new List<string>();
            List<string> OneLinks = new List<string>();
            List<string> ZippyLinks = new List<string>();
            List<string> MagnetLinks = new List<string>();

            foreach (string link in PostFetcher.MultiUP)
            {
                MultiLinks.Add(link);
            }

            foreach (string link in PostFetcher.Onefichier)
            {
                OneLinks.Add(link);
            }

            foreach (string link in PostFetcher.Zippyshare)
            {
                ZippyLinks.Add(link);
            }

            foreach (string link in PostFetcher.Magnet)
            {
                MagnetLinks.Add(link);
            }

            PostFetchingClass Output = new PostFetchingClass();
            Output.Title = args[2];
            Output.URL1 = MagnetLinks;
            Output.URL2 = MultiLinks;
            Output.URL3 = ZippyLinks;
            Output.URL4 = OneLinks;

            string FilePath = args[1];

            string jsonString = JsonSerializer.Serialize(Output);
            File.WriteAllText(FilePath, jsonString);

            string DonePath = args[3];
            File.WriteAllText(DonePath, "True");
        }
    }

    public class PostFetcher
    {
        public static List<string> OpenerLinks = new List<string>();

        public static List<string> MultiUP = new List<string>();
        public static List<string> Onefichier = new List<string>();
        public static List<string> Zippyshare = new List<string>();
        public static List<string> Magnet = new List<string>();

        public static List<string> DLCList = new List<string>();

        public static void GetLinks(string PostURL)
        {
            var options = new ChromeOptions()
            {
                BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            };

            options.AddArguments(new List<string>() { "headless" });
            var browser = new ChromeDriver(options);

            browser.Navigate().GoToUrl(PostURL);
            var PostLinks = browser.FindElements(By.ClassName("postlink"));


            foreach(var PostLink in PostLinks)
            {
                string E = PostLink.GetAttribute("href");

                if (E.Substring(0, 18) == "http://dl.kaoskrew" || E.Substring(0, 19) == "https://dl.kaoskrew")
                {
                    string T = PostLink.GetAttribute("text");
                    
                    if(T.Substring(0, 7).ToLower() == "multiup")
                    {
                        OpenerLinks.Add(E);
                    }
                    else if (T.Substring(0, 10).ToLower() == "zippyshare")
                    {
                        OpenerLinks.Add(E);
                    }
                    else if(T.Substring(0, 8).ToLower() == "1fichier" || T.Substring(0, 10).ToLower() == "onefichier")
                    {
                        OpenerLinks.Add(E);
                    }
                    else if(T.Substring(0, 6).ToLower() == "magnet")
                    {
                        OpenerLinks.Add(E);
                    }
                }
                else
                {
                    //Store
                    //Zoom
                    //Discord
                    if(E.Substring(0, 13) != "https://store")
                    {
                        if(E.Substring(0, 15) != "https://discord")
                        {
                            if (E.Substring(0, 13) != "https://z-o-o")
                            {
                                OpenerLinks.Add(E);
                            }
                        }
                    }

                }
            }

            foreach(string OpenerLink in OpenerLinks)
            {               
                if(OpenerLink.Substring(0, 17) == "https://filecrypt")
                {
                    browser.Navigate().GoToUrl(OpenerLink);

                    var DLCLink = browser.FindElements(By.ClassName("dlcdownload"));

                    foreach (var DLC in DLCLink)
                    {
                        string e = DLC.GetAttribute("onclick");
                        string[] DLCUneditedList = e.Split("\'");
                        string DLCID = DLCUneditedList[1];

                        string DLC_URL = "https://www.filecrypt.cc/DLC/" + DLCID + ".dlc";

                        DLCList.Add(DLC_URL);

                        string PlaceforDLC = "C:\\Users\\Disstract\\Downloads\\Test.dlc";

                        using (var client = new WebClient()){
                            client.DownloadFile(DLC_URL, PlaceforDLC);
                        }

                        string DLCContent = "";

                        if (File.Exists(PlaceforDLC))
                        {
                            DLCContent = File.ReadAllText(PlaceforDLC);
                        }

                        using (WebClient wc = new WebClient())
                        {
                            wc.Headers.Add("User-Agent", "Disstract-ProjectBlackPearl + https://gitlab.com/project-black-pearl/project-black-pearl");
                            wc.Headers.Add("From", "projectblackpearl@protonmail.com");

                            string HtmlResult = wc.UploadString("https://cable.ayra.ch/decrypt/decrypt.php?mode=dlc", DLCContent);
                            DecryptorClass decryptedDLC = JsonSerializer.Deserialize<DecryptorClass>(HtmlResult)!;

                            foreach(var data in decryptedDLC.data)
                            {
                                if(data.Substring(0, 15) == "https://multiup")
                                {
                                    MultiUP.Add(data);
                                }
                                else if(data.Substring(0, 16) == "https://1fichier")
                                {
                                    Onefichier.Add(data);
                                }
                                else if(data.Contains("zippyshare") || data.Contains("Zippyshare"))
                                {
                                    Zippyshare.Add(data);
                                }
                                else if (data.Substring(0, 6) == "magnet")
                                {
                                    Magnet.Add(data);
                                }
                            }                            
                        }
                    }
                }

                else if(OpenerLink.Substring(0, 13) == "https://paste")
                {
                    browser.Navigate().GoToUrl(OpenerLink);
                    Thread.Sleep(100);

                    var thingaroo = browser.FindElements(By.TagName("a"));

                    foreach(var thing in thingaroo)
                    {
                        string t = thing.GetAttribute("href");
                        if(t.Substring(0, 6) == "magnet" || t.Substring(0, 5) == "magnet")
                        {
                            Magnet.Add(t);
                        }
                    }
                }

                else if (OpenerLink.Substring(0, 18) == "http://dl.kaoskrew" || OpenerLink.Substring(0, 19) == "https://dl.kaoskrew")
                {
                    browser.Navigate().GoToUrl(OpenerLink);
                    Thread.Sleep(2000);
                    var GoestoFly = browser.FindElements(By.ClassName("mwButton"));

                    foreach(var Fly in GoestoFly)
                    {
                        string SkippedAdfly = Fly.GetAttribute("href");
                        Console.WriteLine(SkippedAdfly);

                        if (SkippedAdfly.Substring(0, 15) == "https://multiup")
                        {
                            MultiUP.Add(SkippedAdfly);
                        }
                        else if (SkippedAdfly.Substring(0, 16) == "https://1fichier")
                        {
                            Onefichier.Add(SkippedAdfly);
                        }
                        else if (SkippedAdfly.Contains("zippyshare") || SkippedAdfly.Contains("Zippyshare"))
                        {
                            Zippyshare.Add(SkippedAdfly);
                        }
                        else if (SkippedAdfly.ToLower().Contains("magnet"))
                        {
                            Magnet.Add(SkippedAdfly);
                        }
                    }
                }
            }

            browser.Quit();
        }
    }
}
