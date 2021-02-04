using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TwitterAutoScraper
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Press A for Auto Scrap, any other keys for Manual Scrap");
            var selectedMode = Console.ReadLine();

            if (selectedMode != null && string.Equals(selectedMode.ToLower(), "a"))
            {
                AutoScrap();
            }
            else
            {
                ManualScrap();
            }

            Console.WriteLine("Done, press any key to exit");
            Console.ReadKey();
        }

        private static void AutoScrap()
        {
            long height = 0;
            List<Tweet> tweets = new List<Tweet>();
            IWebDriver driver = new ChromeDriver();
            //driver.Url = "https://twitter.com/search?l=en&q=delays%20from%3Ajubileeline&src=typd";
            driver.Url = "https://twitter.com/search?l=en&q=happy%20from%3Ajudykang&src=typd"; //twitter advance serach url
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            do
            {
                var newHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");
                if (newHeight > 3000) break;
                height = newHeight;
                Thread.Sleep(2000);
            } while (true);

            var timeStampContents = driver.FindElements(By.ClassName("tweet-timestamp"));
            var tweetTextContents = driver.FindElements(By.ClassName("js-tweet-text-container"));

            for (int i = 0; i < timeStampContents.Count(); i++)
            {
                string message = tweetTextContents[i].Text
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace(",", string.Empty)
                    .Replace(";", string.Empty);

                tweets.Add(new Tweet
                {
                    TimeStamp = timeStampContents[i].Text,
                    Message = message
                });

                Console.WriteLine(tweets[tweets.Count - 1].TimeStamp);
                Console.WriteLine(tweets[tweets.Count - 1].Message);
            }

            driver.Close();
            driver.Quit();

            SaveToCsv(tweets);
        }

        private static void ManualScrap()
        {
            var web = new HtmlWeb();
            var document = web.Load(Path.GetFullPath("test1.html")); //twitter advance seach html
            var page = document.DocumentNode;

            List<Tweet> tweets = new List<Tweet>();

            foreach (var item in page.QuerySelectorAll(".content"))
            {
                if ((item.QuerySelector(".tweet-timestamp") != null) && (item.QuerySelector(".js-tweet-text-container") != null))
                {
                    //get timestamp
                    var timestampHtml = item.QuerySelector(".tweet-timestamp").OuterHtml;
                    int startPos = timestampHtml.IndexOf("title") + 7;
                    var timestampSub = timestampHtml.Substring(startPos, 25).Split('"');

                    //clean the twitter html format
                    string message = item.QuerySelector(".js-tweet-text-container").InnerText
                        .Replace("\n", string.Empty)
                        .Replace(",", string.Empty)
                        .Replace(";", string.Empty)
                        .Replace("\r", string.Empty)
                        .Remove(0, 2);

                    message += "\r";

                    tweets.Add(new Tweet
                    {
                        TimeStamp = timestampSub[0],
                        Message = message
                    });

                    Console.WriteLine(tweets[tweets.Count - 1].TimeStamp);
                    Console.WriteLine(tweets[tweets.Count - 1].Message);
                }
            }
            SaveToCsv(tweets);
        }

        private static void SaveToCsv(IEnumerable<Tweet> tweets)
        {
            //config
            string filePath = @"E:\results.csv";
            string delimiter = ",";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, "DateTime", "Message")); //header

            foreach (var tweet in tweets)
            {
                sb.AppendLine(string.Join(delimiter, tweet.TimeStamp, tweet.Message));
            }
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}