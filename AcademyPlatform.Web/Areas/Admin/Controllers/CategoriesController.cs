namespace AcademyPlatform.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Courses;

    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;


        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: /Categories/
        [HttpGet]
        public ViewResult Index()
        {
            return View(_categoryService.GetAll<CategoryListViewModel>());
        }

        // GET: /Categories/Details/5
        [HttpGet]
        public ViewResult Details(int id)
        {
            return View(_categoryService.GetById<CategoryEditViewModel>(id));
        }

        // GET: /Categories/Create
        [HttpGet]
        public ActionResult Create()
        {
            SetRelatedItemsInViewBag();
            return View();
        }

        // POST: /Categories/Create
        [HttpPost]
        public ActionResult Create(CategoryEditViewModel category)
        {
            if (ModelState.IsValid)
            {
                var domainModel = Mapper.Map<Category>(category);
                _categoryService.Create(domainModel);
                return RedirectToAction("Index");
            }
            else
            {
                SetRelatedItemsInViewBag();
                return View(category);
            }
        }

        // GET: /Categories/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetRelatedItemsInViewBag();
            return View(_categoryService.GetById<CategoryEditViewModel>(id));
        }

        // POST: /Categories/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CategoryEditViewModel category)
        {
            if (ModelState.IsValid)
            {
                var categoryInDb = _categoryService.GetById(id);
                var updatedModel = Mapper.Map(category, categoryInDb);
                _categoryService.Update(updatedModel);
                return RedirectToAction("Index");
            }
            else
            {
                SetRelatedItemsInViewBag();
                return View(category);
            }
        }

        // GET: /Categories/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(_categoryService.GetById<CategoryEditViewModel>(id));
        }

        // POST: /Categories/Delete/5

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _categoryService.Delete(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _categoryService.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetRelatedItemsInViewBag()
        {
        }
    }
}
