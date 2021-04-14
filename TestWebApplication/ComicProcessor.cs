using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using TestWebApplication.Helpers;
using TestWebApplication.Models;

namespace TestWebApplication
{
    public class ComicProcessor
    {
        public static int MacComicNumber { get; set; }

        public async static Task<Comic> LoadComic(int comicNumber = 0)
        {
            string url = "";
            if (comicNumber > 0)
            {
                url = $"https://xkcd.com/{ comicNumber }/info.0.json";
            }
            else
            {
                url = "https://xkcd.com/info.0.json";
            }

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Comic comic = await response.Content.ReadAsAsync<Comic>();
                    
                    if (comicNumber ==0)
                    {
                        MacComicNumber = comic.Num;
                    }
                    return comic;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }            
        }        
    }
}