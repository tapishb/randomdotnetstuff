using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestWebApplication.Helpers;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    public class DocumentController : ApiController
    {
        List<Document> documentlist = new List<Document>();
        Cookie cookie;        

        public DocumentController()
        {
            ApiHelper.InitializeClient();
        }
                
        public async Task<Cookie> Get(string username, string password)
        {
            await LoadCookie(username, password);

            string token = "epres-usr-107=" + cookie.AccessToken;
            //await LoadDocuments(cookie);
            return cookie;
        }

        public async Task<List<SourceDocument>> Get(int DegreeOfParallelism = 1)
        {
            //string jsonFile = @"C:\Users\TB1440\source\repos\WebApplication\TestWebApplication\DataStore\Documents.json";

            //var json = File.ReadAllText(jsonFile);

            var json = JArray.Parse(File.ReadAllText(@"C:\Users\TB1440\source\repos\WebApplication\TestWebApplication\DataStore\Documents1000.json"));
            var serializer = new JsonSerializer();
            List<SourceDocument> docList = serializer.Deserialize<List<SourceDocument>>(new JTokenReader(json));
            List<Task<(string, byte[])>> listOfTasks = new List<Task<(string, byte[])>>();
            var throttler = new SemaphoreSlim(initialCount: DegreeOfParallelism);
            Dictionary<string, byte[]> fileNameContent = new Dictionary<string, byte[]>();
            foreach (SourceDocument document in docList)
            {
                await throttler.WaitAsync();
                listOfTasks.Add(DocumentPocProcessor.LoadPdfFromSource(document, throttler));
            }

            int _cnt = 0;
            foreach (Task<(string, byte[])> task in listOfTasks)
            {
                (string _file, byte[] _content) = await task;
                fileNameContent.Add((_cnt++) + "_" + _file, _content);
                //fileNameContent.Add(_file, _content);
            }

            //await Task.WhenAll(listOfTasks);
            DocumentPocProcessor.ZipFiles(fileNameContent);
            return docList;
        }

        public async Task<List<Document>> Get(string token)
        {
            
            //await LoadCookie(username, password);

            //string cookie = "epres-usr-107=" + credentials.AccessToken;
            //string cookie = "epres-usr-107=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImVwcmVzLXVzciJ9.eyJyb2wiOiJ1c2VyIiwiY2xpIjoiMTA3IiwiY2xpX2x2MiI6IjEwNyIsInVzZXJfaWQiOiJmYzY3MDBlYi1hNzQwLWViMTEtYTJmOC0wMDUwNTZiODE1MjIiLCJudW0iOiIyMjIyMjIyMjIyIiwiYW1yIjoiRFNPIiwidXNyIjoiMjIyMjIyMjIyMiIsIm5iZiI6MTYwOTg5NTkxMiwiZXhwIjoxNjA5OTgyMzEyLCJpc3MiOiJlcHJlcy11c3IiLCJhdWQiOiJlcHJlcy1jdXN0b21lci1wb3J0YWwiLCJqdGkiOjEwMTk0MX0.u6WBEgQZJAi-Ht9wg9cK4l2AAbjdee-Ddvp12L4GJwA";
            
            await LoadDocuments(token);

            List<Task<(string, byte[])>> listOfTasks = new List<Task<(string, byte[])>>();

            Dictionary<string, byte[]> fileNameContent = new Dictionary<string, byte[]>();
            foreach (Document document in documentlist)
            {
                //listOfTasks.Add(fileNameContent.Add((fileName, fileContent) => (
                //{
                //    (fileName, fileContent) = DocumentPocProcessor.LoadPdf(token, document)
                //}
                //));                
                listOfTasks.Add(DocumentPocProcessor.LoadPdf(token, document));
            }

            Console.WriteLine("Done with LoadPDF");
            foreach (Task<(string, byte[])> task in listOfTasks)
            {
                (string _file, byte[] _content) = await task;
                fileNameContent.Add(_file, _content);
            }

            //await Task.WhenAll(listOfTasks);
            DocumentPocProcessor.ZipFiles(fileNameContent);

            return documentlist;
        }

        private async Task LoadCookie(string username, string password)
        {
            cookie = await DocumentPocProcessor.LoadCookie(username, password);
        }

        //private async Task LoadPdf(string token)
        //{

        //    Parallel.ForEach(documentlist, (document) =>
        //    {
        //        DocumentPocProcessor.LoadPdf(token, document);
        //    });
            
        //}

        private async Task LoadDocuments(string cookie)
        {
            documentlist = await DocumentPocProcessor.LoadDocuments(cookie);
        }
    }
}