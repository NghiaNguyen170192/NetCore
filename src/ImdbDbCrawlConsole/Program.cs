using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ImdbDbCrawlConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ImdbCrawler crawler = new ImdbCrawler();
            crawler.RunAsync();

            Console.ReadLine();
        }
    }
}
