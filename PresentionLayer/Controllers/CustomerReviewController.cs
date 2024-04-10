using DataAccessLayer.Entites;
using DataAccessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentionLayer.Controllers
{
    public class CustomerReviewController : Controller
    {
        private readonly IGenricRepository<CustomerReview> _customer;
        public CustomerReviewController(IGenricRepository<CustomerReview> customer)
        {
            _customer = customer;
        }

        // GET: CustomerReviewController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerReviewController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerReviewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var review = new CustomerReview()
                {
                    name = viewModel.name,
                    email = viewModel.email,
                    phone = viewModel.phone,
                    message = viewModel.message,
                };
                _customer.Create(review);
                _customer.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

    }
}
