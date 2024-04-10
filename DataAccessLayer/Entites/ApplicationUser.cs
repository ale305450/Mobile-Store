using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class ApplicationUser: IdentityUser
    {
        //Propties
        public string fullName { get; set; }
        public string address { get; set; }
    }
}
