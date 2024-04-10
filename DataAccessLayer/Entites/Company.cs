using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class Company
    {
        //Propties
        public Guid Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public byte[] image { get; set; }
    }
}
