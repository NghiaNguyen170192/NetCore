using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ImdbDbCrawlConsole
{
    public class ImdbCrawler
    {
        private const string IMDB_URL = "https://datasets.imdbws.com/";
        private const string DOWNLOAD_DIRECTORY = "DownloadFiles";
        private const string EXTRACT_DIRECTORY = "ExtractedFiles";

        public async void RunAsync()
        {
            var downloadUrls = GetDownloadUrls();
            await DownloadFilesAsync(downloadUrls);
            ExtractFiles();
        }


        private void ExtractFiles()
        {
            DirectoryInfo directorySelected = new DirectoryInfo(GetDirectory(DOWNLOAD_DIRECTORY));

            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
            {
                ExtractFile(fileToDecompress);
            }
        }

        private void ExtractFile(FileInfo file)
        {
            using (FileStream originalFileStream = file.OpenRead())
            {
                string fileName = file.Name.Remove(file.Name.Length - file.Extension.Length);
                string filePath = Path.Combine(GetDirectory(EXTRACT_DIRECTORY), fileName);

                using (FileStream decompressedFileStream = File.Create(filePath))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        private async Task DownloadFilesAsync(List<string> downloadUrls)
        {
            string downloadPath = GetDirectory(DOWNLOAD_DIRECTORY);

            foreach (var downloadUrl in downloadUrls)
            {
                string filePath = GetDownloadFilePath(downloadUrl, downloadPath);
                await DownloadFileAsync(downloadUrl, filePath);
            }
        }

        private async Task DownloadFileAsync(string downloadUrl, string filePath)
        {
            if (!File.Exists(filePath))
            {
                using (WebClient webClient = new WebClient())
                {
                    await webClient.DownloadFileTaskAsync(downloadUrl, filePath);
                }
            }
        }

        private string GetDirectory(string directoryName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), directoryName);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private string GetDownloadFilePath(string downloadUrl, string downloadDirectory)
        {
            string fileName = downloadUrl.Replace(IMDB_URL, "");
            string filePath = Path.Combine(downloadDirectory, fileName);
            return filePath;
        }

        private List<string> GetDownloadUrls()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(IMDB_URL);
            List<string> downloadUrls = new List<string>();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(html);
                    HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//a");

                    foreach (HtmlNode link in collection)
                    {
                        string target = link.Attributes["href"].Value;
                        if (target.Contains(IMDB_URL))
                        {
                            downloadUrls.Add(target);
                        }
                    }
                }
            }

            return downloadUrls;
        }
    }
}
