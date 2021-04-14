using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestWebApplication.Helpers;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    ///<summary>
    ///This is where the controller lies
    ///</summary>
    public class PeopleController : ApiController
    {
        List<Person> people = new List<Person>();

        public PeopleController()
        {
            ApiHelper.InitializeClient();
            people.Add(new Person { FirstName = "Tapish", LastName = "Baronia", Id = 1 });
            people.Add(new Person { FirstName = "Riaan", LastName = "Baronia", Id = 2 });
            people.Add(new Person { FirstName = "Ayaan", LastName = "Baronia", Id = 3 });
            people.Add(new Person { FirstName = "Tuhina", LastName = "Baronia", Id = 4 });
        }
        
        /// <summary>
        /// This is get call
        /// </summary>
        /// <returns></returns>
        // GET: api/People
        public List<Person> Get()
        {
            return people;
        }

        // GET: api/People/5
        public Person Get(int id)
        {
            return people.Where(x => x.Id == id).FirstOrDefault();
        }

        // POST: api/People
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/People/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/People/5
        public void Delete(int id)
        {
        }
    }
}
