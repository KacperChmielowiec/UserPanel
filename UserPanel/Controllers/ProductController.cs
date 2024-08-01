using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Attributes;
using UserPanel.Interfaces;
using UserPanel.Models.Product;
using UserPanel.Types;

namespace UserPanel.Controllers
{
    [ProductFilter]
    public class ProductController : Controller
    {
        IDataBaseProvider _provider;
        public ProductController(IDataBaseProvider dataBaseProvider) {
            _provider = dataBaseProvider;
        }

        [Authorize]
        [HttpGet("/campaign/product/list/{id}")]
        public IActionResult Index(Guid id)
        {
            List<Product> products = _provider.GetProductRepository().GetProductsByCampId(id);

            return View(new ProductListModelView() { id_camp = id, Products = products});
        }

        [Authorize]
        [HttpPost("/campaign/product/remove/{id}")]
        public IActionResult Remove(Guid id)
        {
            try
            {
                if(_provider.GetProductRepository().GetProductsByCampId(id)?.Count() == 0 )
                {
                    return RedirectToAction("Index", new { id = id });
                }

                _provider.GetProductRepository().RemoveProductsByCampId(id);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { id = id, error = ErrorForm.err_remove });
            }

            return RedirectToAction("Index", new { id = id, success = ErrorForm.suc_remove } );
        }
    }
}
