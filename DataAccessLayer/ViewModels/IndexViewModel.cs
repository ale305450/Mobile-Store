using DataAccessLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels
{
    public class IndexViewModel
    {
        public List<Product> mobileProducts { get; set; }
        public List<Product> smartWatches { get; set; }
        
    }
}
