using DataAccessLayer.Entites;
using DataAccessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentionLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IGenricRepository<Product> _product;
        private readonly IGenricRepository<Company> _company;
        private readonly IGenricRepository<Category> _category;
        public ProductController(
            IGenricRepository<Product> product,
            IGenricRepository<Company> company,
            IGenricRepository<Category> category
            )
        {
            _product = product;
            _company = company;
            _category = category;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            var products = _product.GetAll().Select(p =>
            {
                return new ProductViewModel
                {
                    Id = p.Id,
                    name = p.name,
                    description = p.description,
                    price = p.price,
                    image = p.image,
                    categoryId = p.categoryId,
                    categoryName = _category.GetById(p.categoryId).Name,
                    companyId = p.companyId,
                    companyName = _company.GetById(p.companyId).name
                };
            }).ToList();
            return View(products);
        }

        // GET: ProductController/Details/5
        [Authorize(Roles = "Customer,Admin")]
        public ActionResult Details(Guid id)
        {
            var product = _product.GetById(id);
            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                name = product.name,
                price = product.price,
                description = product.description,
                image = product.image,
                companyId = product.companyId,
                companyName = _company.GetById(product.companyId).name,
                categoryId = product.categoryId,
                categoryName = _category.GetById(product.categoryId).Name
            };
            return View(viewModel);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            ViewBag.Companines = new SelectList(_company.GetAll(), "Id", "name");
            ViewBag.Categories = new SelectList(_category.GetAll(), "Id", "Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //Check if the user uploaded any image
                if (Request.Form.Files.Count == 0)
                {
                    ViewBag.missingImage = "The product image is missing";
                    ViewBag.Companines = new SelectList(_company.GetAll(), "Id", "name");
                    ViewBag.Categories = new SelectList(_category.GetAll(), "Id", "Name");
                    return View();
                }
                // Check if there product with the same name
                var checkName = _product.GetAll().Where(c => c.name == viewModel.name).ToList();
                if (checkName.Any())
                {
                    ViewBag.CheckName = "There is product with that name";
                    ViewBag.Companines = new SelectList(_company.GetAll(), "Id", "name");
                    ViewBag.Categories = new SelectList(_category.GetAll(), "Id", "Name");
                    return View();
                }
                var newMobile = new Product()
                {
                    name = viewModel.name,
                    description = viewModel.description,
                    image = uploadImage(viewModel.image),
                    price = viewModel.price,
                    companyId = viewModel.companyId,
                    categoryId = viewModel.categoryId,
                };
                _product.Create(newMobile);
                _product.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Companines = new SelectList(_company.GetAll(), "Id", "name");
                ViewBag.Categories = new SelectList(_category.GetAll(), "Id", "Name");
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(Guid id)
        {
            //Get All Companies 
            ViewBag.Companines = new SelectList(_company.GetAll(), "Id", "name");
            //Get All Categories
            ViewBag.Categories = new SelectList(_category.GetAll(), "Id", "Name");

            var exisitProduct = _product.GetById(id);
            //Fill the form with exisiting Product information
            var product = new ProductViewModel()
            {
                name = exisitProduct.name,
                description = exisitProduct.description,
                image = exisitProduct.image,
                price = exisitProduct.price,
                companyId = exisitProduct.companyId,
                categoryId = exisitProduct.categoryId,
            };
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var updatedProduct = _product.GetById(id);
                // Check if there product with the same name
                var checkName = _product.GetAll().Where(c => c.name == viewModel.name).ToList();
                if (checkName.Any())
                {
                    ViewBag.CheckName = "There is product with that name";
                    return RedirectToAction("Edit",id);
                }
                updatedProduct.name = viewModel.name;
                updatedProduct.description = viewModel.description;
                updatedProduct.price = viewModel.price;
                updatedProduct.companyId = viewModel.companyId;
                updatedProduct.categoryId = viewModel.categoryId;

                //Check if the image changed or not
                var updatedProductImage = uploadImage(viewModel.image);
                if (updatedProductImage != null)
                    updatedProduct.image = updatedProductImage;

                _product.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //Get All Companies 
                ViewBag.Companines = new SelectList(_company.GetAll(), "Id", "name");
                //Get All Categories
                ViewBag.Categories = new SelectList(_category.GetAll(), "Id", "Name");
                return View(viewModel);
            }
        }

        // GET: ProductController/Delete/5
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                _product.Delete(id);
                _product.Save();
            }
            return RedirectToAction(nameof(Index));
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
