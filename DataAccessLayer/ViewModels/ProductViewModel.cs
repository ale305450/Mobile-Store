using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels
{
    public class ProductViewModel
    {
        //Propties
        public Guid Id { get; set; }
        [Required][Display(Name = "Product Name :")]
        public string name { get; set; }
        [Display(Name = "Product information :")]
        public string description { get; set; }
        [Display(Name = "Product price :")]
        public int price { get; set; }
        [Display(Name = "Product image :")]
        public byte[] image { get; set; }

        //Relationship
        [Display(Name = "Product's company :")]
        public Guid companyId { get; set; }
        [Display(Name = "Company Name :")]
        public string companyName { get; set; }

        [Display(Name = "Product's category :")]
        public Guid categoryId { get; set; }
        [Display(Name = "Category Name :")]
        public string categoryName { get; set; }
    }
}
