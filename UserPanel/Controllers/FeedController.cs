using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using UserPanel.Attributes;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Feed;
using UserPanel.Models.Adverts;
using UserPanel.Models.Product;
using UserPanel.Services;
using UserPanel.Types;
namespace UserPanel.Controllers
{

    [FeedFilter]
    public class FeedController : Controller
    {
        IDataBaseProvider _dataBaseProvider;
        IOptions<RenderModel> _defaultRender;
        public FeedController(IDataBaseProvider dataBaseProvider, IOptions<RenderModel> render) {
            _dataBaseProvider = dataBaseProvider;
            _defaultRender = render;
        }
  
        [HttpPost("campaign/feed/add")]
        public IActionResult Index([FromForm] string url, [FromForm] FeedTypes format, [FromForm] Guid id)
        {
            if (url.IsNullOrEmpty())
            {
                return RedirectToAction("List", new { id = id, error = ErrorForm.err_feed_url });
            }
            if(!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }

            try
            {
                List<Product> products = new List<Product>();

                XmlDocument doc = new XmlDocument();
                doc.Load(url);

                products = FeedParser.ParseFeed(format, doc);


                foreach (Product product in products)
                {
                    int errors = ValidatorModel.ValidateModel(product).Count;
                    if (errors > 0)
                    {
                        throw new ValidationException("Parsed models did not met requires");
                    }
                }

                _dataBaseProvider.GetProductRepository().AddProductsByCampId(products, id);
                _dataBaseProvider.GetFeedRepository().AddFeedByCampId(new Feed() { Updated = DateTime.Now, Created = DateTime.Now, FeedType = format, Id = Guid.NewGuid(), Url = url }, id);

                return RedirectToAction("List", new { id = id, success = ErrorForm.suc_create });

            }
            catch (ValidationException)
            {
                return RedirectToAction("List", new { id = id, error = ErrorForm.err_validation });
            }
            catch(SqlException) 
            {
                return RedirectToAction("List", new { id = id, error = ErrorForm.err_create });
            }
            catch(Exception)
            {
                return RedirectToAction("List", new { id = id, error = ErrorForm.err_parse });
            }
            
        }

        [HttpGet("campaign/advert/preview/render")]
        public IActionResult Preview([FromQuery] RenderModel renderModel)
        {
            try
            {
                if (renderModel == null)
                {
                    renderModel = _defaultRender.Value;
                }
                if (renderModel.MainUrl.IsNullOrEmpty())
                {
                    renderModel.MainUrl = _defaultRender.Value.MainUrl;
                }
                if (renderModel.LogoSrc.IsNullOrEmpty())
                {
                    renderModel.LogoSrc = _defaultRender.Value.LogoSrc;
                }
                if (renderModel.Size.IsNullOrEmpty())
                {
                    renderModel.Size = _defaultRender.Value.Size;
                }

                List<Product> products;

                if (renderModel.camp_id == Guid.Empty)
                    products = _dataBaseProvider.GetProductRepository().GetProducts(5);
                else
                    products = _dataBaseProvider.GetProductRepository().GetProductsByCampId(renderModel.camp_id);

                renderModel.Products = products;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return View(renderModel);
        }

        [HttpGet("campaign/advert/render")]
        public IActionResult Render([FromQuery] Guid ad, Guid ad_f)
        {
            var AdvertModel = _dataBaseProvider.GetAdvertDyRepository().GetAdvertById(ad);
            if (AdvertModel == null) return NotFound();
            if (!AdvertModel.Formats.Any(f => f.Id == ad_f)) return NotFound();

            var format = AdvertModel.Formats.FirstOrDefault(f => f.Id == ad_f);

            RenderModel renderModel = new RenderModel();

            renderModel.MainUrl = format.Url;
            renderModel.LogoSrc = "";
            renderModel.Size = format.Size;
            renderModel.camp_id = PermissionActionManager<Guid>.GetFullPath(ad).Camp;

            var products = _dataBaseProvider.GetProductRepository().GetProductsByCampId(renderModel.camp_id);

            renderModel.Products = products;

            return View("Preview", renderModel);
        }

        [HttpGet("campaign/feed/list/{id}")]
        public IActionResult List(Guid id)
        {
            List<Feed> feeds = _dataBaseProvider.GetFeedRepository().GetFeedsByCampId(id);
            return View(new FeedsListViewModel() { Feeds = feeds, id_camp = id });
        }

        [HttpPost("campaign/feed/remove/{id}/{id_camp}")]
        public IActionResult Remove(Guid id, Guid id_camp)
        {
            try
            {
                _dataBaseProvider.GetFeedRepository().RemoveFeedById(id);

            }catch (Exception)
            {
                return RedirectToAction("List", new { id = id_camp, error = ErrorForm.err_remove });
            }

            return RedirectToAction("List", new { id = id_camp, success = ErrorForm.suc_remove });
        }

        [HttpPost("campaign/feed/refresh/{id}/{id_camp}")]
        public IActionResult Refresh(Guid id, Guid id_camp)
        {
            try
            {
                var Feed = _dataBaseProvider.GetFeedRepository().GetFeedsByCampId(id_camp)?.Where(f => f.Id == id).FirstOrDefault();

                if (Feed == null) return NotFound();

                string url = Feed.Url;
                FeedTypes format = Feed.FeedType;

                List<Product> products = new List<Product>();

                XmlDocument doc = new XmlDocument();
                doc.Load(url);

                products = FeedParser.ParseFeed(format, doc);

                foreach (Product product in products)
                {
                    int errors = ValidatorModel.ValidateModel(product).Count;
                    if (errors > 0)
                    {
                        throw new ValidationException("Parsed models did not met requires");
                    }
                }

                _dataBaseProvider.GetProductRepository().AddProductsByCampId(products, id_camp);

            }
            catch (Exception)
            {
                return RedirectToAction("List", new { id = id_camp, error = ErrorForm.err_feed_refresh });
            }

            return RedirectToAction("List", new { id = id_camp, success = ErrorForm.suc_feed_refresh });
        }

    }
}
