using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels
{
    public class CompanyViewModel
    {
        //Propties
        public Guid Id { get; set; }
        [Required][Display(Name ="Enter Company name :")]
        public string name { get; set; }
        [Display(Name = "Enter Company description :")]
        public string description { get; set; }
        [Display(Name = "Upload Company image :")]
        public byte[] image { get; set; }
    }
}
