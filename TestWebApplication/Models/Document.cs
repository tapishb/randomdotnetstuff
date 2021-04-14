using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApplication.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public string DocumentApiPath { get; set; }
        public int AccountNumber { get; set; }
    }
}