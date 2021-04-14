using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TestWebApplication.Helpers;
using TestWebApplication.Models;

namespace TestWebApplication
{
    public class DocumentPocProcessor
    {
        public async static Task<Cookie> LoadCookie(string username, string password)
        {
            //string url = "https://afcu.test.myepresentment.com/api/DsoLoginFlow/login";
            string url = "https://ptsv2.com/t/n6rgr-1610422891/post";

            Cookie cookie = new Cookie() { AccessToken = "", Message = "" };

            var credentials = new Credentials() { UserName = username, Password = password };

            var stringCredentials = JsonConvert.SerializeObject(credentials);

            //var payload = new StringContent("{\"UserName\":\"2222222222\",\"Password\":\"qwertyuiopasdf\"}", Encoding.UTF8, "application/json");
            HttpContent payload = new StringContent(stringCredentials, Encoding.UTF8, "application/json");

            ApiHelper.ApiClient.DefaultRequestHeaders.Add("X-Correlation-ID", "8a7f46f4-7b53-41fe-86ff-ad2f6e24a203");

            try
            {
                //using (response = await ApiHelper.ApiClient.GetAsync(url))
                //using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, payload))
                //using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync(url, "{\"UserName\":\"2222222222\",\"Password\":\"qwertyuiopasdf\"}"))
                using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, payload))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    cookie = JsonConvert.DeserializeObject<Cookie>(result);
                    if (response.IsSuccessStatusCode)
                    {
                        return cookie;
                    }
                    else
                    {
                        return cookie;
                    }
                }
            }
            catch (Exception e)
            {
                cookie.Message = string.Format("RestHttpClient.SendRequest failed: {0}", e);
                return cookie;
            }
        }
        public async static Task<List<Document>> LoadDocuments(string cookie)
        {
            //string url = "customer/documentfeed";
            string url = "https://crcu.test.myepresentment.com/api/customer/documentfeed";
            //using (HttpResponseMessage response = await ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new HttpCookie(cookie));

            ApiHelper.ApiClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            ApiHelper.ApiClient.DefaultRequestHeaders.Add("Cookie", cookie);

            
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<Document> documentList;
                    var result = await response.Content.ReadAsStringAsync();

                    documentList = JsonConvert.DeserializeObject<List<Document>>(result);
                    return documentList;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }



        public static void ZipFiles(Dictionary<string, byte[]> files)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Update))
                {
                    foreach (var file in files)
                    {
                        ZipArchiveEntry orderEntry = archive.CreateEntry(file.Key); //create a file with this name
                        using (BinaryWriter writer = new BinaryWriter(orderEntry.Open()))
                        {
                            writer.Write(file.Value); //write the binary data
                        }
                    }
                }
                //ZipArchive must be disposed before the MemoryStream has data
                System.IO.File.WriteAllBytes(@"C:\PoC\Document.zip", ms.ToArray());
                //return ms.ToArray();
            }
        }

        public async static Task<(string, byte[])> LoadPdfFromSource(SourceDocument document, SemaphoreSlim throttler)
        {
            int documentId = document.SourceDocumentId;
            int accountNo = document.DocumentIndex;
            string url = $"http://pdfserver-test.n.lanvera.org/PDFServer/Document.pdf?DocumentId={ documentId }&AccountNumber={ accountNo }";
            HttpResponseMessage response = new HttpResponseMessage();

            //using (HttpResponseMessage response = await ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new HttpCookie("ss"));
            //ApiHelper.ApiClient.DefaultRequestHeaders.Add("Cookie", cookie);
            ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
            try
            {
                using (response = await ApiHelper.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsByteArrayAsync();
                        System.IO.File.WriteAllBytes(@"C:\PoC\" + documentId + ".pdf", result);
                        //fileNameContent.Add(documentId + ".pdf", result);                        
                        throttler.Release();
                        return (documentId + ".pdf", result);
                        //return result;

                    }
                    else
                    {
                        var result = await response.Content.ReadAsByteArrayAsync();
                        //System.IO.File.WriteAllBytes(@"C:\PoC\" + documentId + "_error.txt", result);
                        throttler.Release();
                        return (documentId + "_error.txt", result);
                        //return result;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(response.ReasonPhrase);

            }
        }

        public async static Task<(string, byte[])> LoadPdf(String cookie, Document document)
        {
            int documentId = document.DocumentId;
            string url = "https://crcu.test.myepresentment.com/api/customer/" + documentId;
            HttpResponseMessage response = new HttpResponseMessage();

            //using (HttpResponseMessage response = await ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new HttpCookie("ss"));
            ApiHelper.ApiClient.DefaultRequestHeaders.Add("Cookie", cookie);
            ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
            try
            {
                using (response = await ApiHelper.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsByteArrayAsync();
                        //System.IO.File.WriteAllBytes(@"C:\PoC\" + documentId + ".pdf", result);
                        //fileNameContent.Add(documentId + ".pdf", result);                        
                        return (documentId + ".pdf", result);
                        //return result;

                    }
                    else
                    {
                        var result = await response.Content.ReadAsByteArrayAsync();
                        //System.IO.File.WriteAllBytes(@"C:\PoC\" + documentId + "_error.txt", result);
                        return (documentId + "_error.txt", result);
                        //return result;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(response.ReasonPhrase);

            }
        }
    }
}