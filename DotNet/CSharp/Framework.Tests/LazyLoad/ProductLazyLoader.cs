﻿using System.Collections.Generic;
using System.Linq;
using Framework.LazyLoad;

namespace Framework.Tests.LazyLoad
{
    public class ProductLazyLoader : LazyLoader<ProductDomain, ProductDomain.Criteria>
    {
        IList<ProductDomain> _products = new List<ProductDomain>();

        public ProductLazyLoader() 
        {
            ProductDomain prod1 = new ProductDomain(null, null);
            ProductDomain prod2 = new ProductDomain(null, null);
            ProductDomain prod3 = new ProductDomain(null, null);
            ProductDomain prod4 = new ProductDomain(null, null);
            ProductDomain prod5 = new ProductDomain(null, null);


            prod1.Id = 1;
            prod1.Description = "Product one";

            prod2.Id = 2;
            prod2.Description = "Product two";

            prod3.Id = 3;
            prod3.Description = "Product three";

            prod4.Id = 4;
            prod4.Description = "Product four";

            prod5.Id = 5;
            prod5.Description = "Product five";


            _products.Add(prod1);
            _products.Add(prod2);
            _products.Add(prod3);
            _products.Add(prod4);
            _products.Add(prod5);
        }

        public override void LoadAllFields(ref ProductDomain entity, ProductDomain.Criteria criteria)
        {
            ProductDomain matchedEntity =  _products
                .FirstOrDefault(x => x.Id == criteria.Id);

            if (matchedEntity == null)
                return;
            
            entity.Id = matchedEntity.Id;
            entity.Description = matchedEntity.Description;
        }
    }
}
