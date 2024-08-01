using UserPanel.Models;
namespace UserPanel.Interfaces.Abstract
{
    public abstract class ProductRepository<T>
    {
        public abstract List<T> GetProducts(int range);
        public abstract List<T> GetProductsByCampId(Guid id);
        public abstract void AddProductsByCampId(List<T> products, Guid id);
        public abstract void RemoveProductsByCampId(Guid id);
    }
}
