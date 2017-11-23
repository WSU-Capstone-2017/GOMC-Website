using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class FailedLoginModel
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public DateTime Date { get; set; }
    }
}