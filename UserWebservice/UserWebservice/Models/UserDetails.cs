using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWebservice.Models
{
    public class UserDetails
    {

        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string AccessCode { get; set; }
        public string FullName { get; set; }
        public long ContactNumber { get; set; }
        public string Address { get; set; }
        public bool Preference { get; set; }
    }
}
