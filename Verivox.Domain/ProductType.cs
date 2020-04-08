using System.Collections.Generic;

namespace Verivox.Domain
{
    public partial class ProductType : BaseEntity
    {
        private ICollection<Product> _products;
        public string Name { get; set; }
        public string Mensuration { get; set; }

        public virtual ICollection<Product> Products
        {
            get => _products ?? (_products = new List<Product>());
            protected set => _products = value;
        }
    }
}
