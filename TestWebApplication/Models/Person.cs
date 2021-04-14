using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApplication.Models
{
    /// <summary>
    /// Represents a person
    /// </summary>
    public class Person
    {
        /// <summary>
        /// this is id
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// this is First name
        /// </summary>
        public string FirstName { get; set; } = "";
        /// <summary>
        /// blah
        /// </summary>
        public string LastName { get; set; } = "";
    }
}