using DataAccessLayer.Entites;
using DataAccessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace PresentionLayer.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CompanyController : Controller
    {
        private readonly IGenricRepository<Company> _company;
        public CompanyController(IGenricRepository<Company> company)
        {
            _company = company;
        }
        // GET: CompanyController
        public ActionResult Index()
        {
            var company = _company.GetAll();
            return View(company);
        }

        // GET: CompanyController/Details/5
        public ActionResult Details(Guid id)
        {
            var company = _company.GetById(id);
            return View(company);
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if there company with the same name
                var checkName = _company.GetAll().Where(c => c.name == viewModel.name).ToList();
                if (checkName.Any())
                {
                    ViewBag.CheckName = "There is company with that name";
                    return View();
                }
                var newCompany = new Company()
                {
                    name = viewModel.name,
                    description = viewModel.description,
                    image = uploadImage(viewModel.image),
                };
                _company.Create(newCompany);
                _company.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: CompanyController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var exisitCompany = _company.GetById(id);
            var company = new CompanyViewModel
            {
                name = exisitCompany.name,
                description = exisitCompany.description,
                image = exisitCompany.image,
            };
            return View(company);
        }

        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, CompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var updatedCompany = _company.GetById(id);
                var checkName = _company.GetAll().Where(c => c.name == viewModel.name).ToList();
                if (checkName.Any())
                {
                    ViewBag.CheckName = "There is company with that name";
                    return RedirectToAction("Edit", id);
                }
                updatedCompany.name = viewModel.name;
                updatedCompany.description = viewModel.description ;
                //Check image if it change or not
                var updatedImage = uploadImage(viewModel.image);
                if (updatedImage != null)
                {
                    updatedCompany.image = updatedImage;
                }
                _company.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(viewModel);
            }
        }

        // GET: CompanyController/Delete/5
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _company.Delete(id);
                _company.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public byte[] uploadImage(byte[] image)
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                using (var memory = new MemoryStream())
                {
                    file.CopyToAsync(memory);
                    image = memory.ToArray();
                }
            }
            return image;
        }
    }
}
