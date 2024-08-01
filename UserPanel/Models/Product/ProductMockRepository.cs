using UserPanel.Helpers;
using UserPanel.Interfaces.Abstract;
using UserPanel.References;

namespace UserPanel.Models.Product
{
    public class ProductMockRepository : ProductRepository<Product>
    {
        ISession _session;
        public ProductMockRepository(ISession Session) {
            _session = Session;
        }
        public override void AddProductsByCampId(List<Product> products, Guid id)
        {
            if (products?.Count() > 0)
            {
                var SessionListModel = _session.GetJson<List<CampProducts>>(SessionKeysReferences.prodKey)?.ToList() ?? new List<CampProducts>();

                var CampProduct = SessionListModel.FirstOrDefault(c => c.id_camp == id);

                if (CampProduct != null)
                {
                    HashSet<string> ids = CampProduct.Products.Select(x => x.Id).ToHashSet();

                    var filtered = products.Where(p => !ids.Contains(p.Id)).ToList();

                    if(filtered.Count() > 0) 
                        CampProduct.Products.AddRange(filtered);

                    SessionListModel.RemoveAll(m => m.id_camp == id);
                    SessionListModel.Add(CampProduct);
                }
                else
                {
                    SessionListModel.Add( new CampProducts() { id_camp = id, Products = products});
                }

                

                _session.SetJson(SessionKeysReferences.prodKey, SessionListModel);
            }
        }

        public override List<Product> GetProducts(int range)
        {
           return _session.GetJson<List<CampProducts>>(SessionKeysReferences.prodKey)
                ?.SelectMany(p => p.Products)
                ?.Take(range)?.ToList() ?? new List<Product>();
        }

        public override List<Product> GetProductsByCampId(Guid id)
        {
            var SessionModel = _session.GetJson<List<CampProducts>>(SessionKeysReferences.prodKey)?.FirstOrDefault(m => m.id_camp == id);
            if(SessionModel != null)
            {
                return SessionModel.Products;
            }
            return new List<Product>(); 
        }

        public override void RemoveProductsByCampId(Guid id)
        {
            var SessionListModel = _session.GetJson<List<CampProducts>>(SessionKeysReferences.prodKey)?.ToList() ?? new List<CampProducts>();
            int elements = SessionListModel.RemoveAll(c => c.id_camp == id);
            if (elements == 0)
            {
                throw new KeyNotFoundException();
            }

            _session.SetJson(SessionKeysReferences.prodKey, SessionListModel);
        }
    }
}
