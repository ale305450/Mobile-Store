﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entites
{
    public class CustomerReview
    {
        //Propties
        public Guid Id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public int phone { get; set; }
        public string message { get; set; }

    }
}
