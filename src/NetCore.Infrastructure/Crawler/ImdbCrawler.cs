using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;
using NetCore.Infrastructure.Models.IMDB;

namespace NetCore.Infrastructure.Crawler
{
    public class ImdbCrawler
    {
        private const string IMDB_URL = "https://datasets.imdbws.com/";
        private const string DOWNLOAD_DIRECTORY = "DownloadFiles";
        private const string EXTRACT_DIRECTORY = "ExtractedFiles";
        
        public async void RunAsync()
        {
            Console.WriteLine("Start");
            var downloadUrls = GetDownloadUrls();
            await DownloadFilesAsync(downloadUrls);
            await ExtractFilesAsync();
            MappingModels();
            Console.WriteLine("End");
        }

        #region Download Files
        private async Task DownloadFilesAsync(List<string> downloadUrls)
        {
            string downloadPath = GetDirectory(DOWNLOAD_DIRECTORY);
            await Task.WhenAll(downloadUrls.Select(downloadUrl => 
                DownloadFileAsync(downloadUrl, GetDownloadFilePath(downloadUrl, downloadPath))));
        }

        private async Task DownloadFileAsync(string downloadUrl, string filePath)
        {
            if (File.Exists(filePath))
            {
                Console.WriteLine("File is already existed");
                return;
            }

            using (WebClient webClient = new WebClient())
            {
                Console.WriteLine($"Downloading: {downloadUrl} at {filePath}");
                await webClient.DownloadFileTaskAsync(downloadUrl, filePath);
                Console.WriteLine($"Downloaded");
            }
        }
        #endregion

        #region Extract Files
        private async Task ExtractFilesAsync()
        {
            DirectoryInfo selectedDirectory = new DirectoryInfo(GetDirectory(DOWNLOAD_DIRECTORY));
            await Task.WhenAll(selectedDirectory.GetFiles("*.gz").Select(fileInfo => ExtractFile(fileInfo)));            
        }

        private async Task ExtractFile(FileInfo file)
        {
            string fileName = file.Name.Remove(file.Name.Length - file.Extension.Length);
            string filePath = Path.Combine(GetDirectory(EXTRACT_DIRECTORY), fileName);

            if (File.Exists(filePath))
            {
                Console.WriteLine($"File is alrerady existed");
                return;
            }

            using (FileStream originalFileStream = file.OpenRead())
            using (FileStream decompressedFileStream = File.Create(filePath))
            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
            {
                Console.WriteLine($"Extracting : {fileName} \n at: {filePath}");
                await decompressionStream.CopyToAsync(decompressedFileStream);
                Console.WriteLine($"Extracted");
            }
        }
        #endregion

        #region Helpers
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
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(IMDB_URL);
            List<string> downloadUrls = new List<string>();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
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
                        Console.WriteLine($"{target}");
                    }
                }
            }
            return downloadUrls;
        }

        private string[] ReadFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath).ToList();
            lines.RemoveAt(0);

            return lines.ToArray();
        }
        #endregion

        #region Mapping
        private void MappingModels()
        {
            DirectoryInfo selectedDirectory = new DirectoryInfo(GetDirectory(EXTRACT_DIRECTORY));

            foreach (FileInfo fileInfo in selectedDirectory.GetFiles("*.tsv"))
            {
                string name = fileInfo.Name;
                IProcessFile processFile;
                switch (name)
                {
                    case "title.akas.tsv":
                        break;
                    case "title.basics.tsv":
                        processFile = new TitleBasicMapper();
                        processFile.ProcessFIle(fileInfo.FullName);
                        break;
                    case "title.crew.tsv":
                        break;
                    case "title.episode.tsv":
                        break;
                    case "title.principals.tsv":
                        break;
                    case "title.ratings.tsv":
                        break;
                    case "name.basics.tsv":
                        break;

                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
