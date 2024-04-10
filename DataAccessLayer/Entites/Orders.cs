using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class Orders
    {
        //Propties
        public Guid Id { get; set; }
        public DateTime orderDate { get; set; }
        public int totalAmout { get; set; }
        public string shippingAddress { get; set; }
        public string statues { get; set; }

        //Relationship
        public string UserId { get; set; }
        public Guid productId { get; set; }
    }
}
