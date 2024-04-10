using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class Product
    {
        //Propties
        public Guid Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public byte[] image { get; set; }

        //Relationship
        public Guid companyId { get; set; }
        public Guid categoryId { get; set; }
    }
}
