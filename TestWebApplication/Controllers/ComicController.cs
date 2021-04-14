using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestWebApplication.Helpers;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    public class ComicController : ApiController
    {
        public ComicController()
        {
            ApiHelper.InitializeClient();
        }

        List<Comic> comics = new List<Comic>();

        public async Task<List<Comic>> Get(int id)
        {
            await LoadImage(id);
            return comics;
        }

        private async Task LoadImage(int imageNumber)
        {
            comics.Add(await ComicProcessor.LoadComic(imageNumber));

        }
    }
}