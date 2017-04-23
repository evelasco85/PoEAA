package com.peaa.LazyLoad;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by aiko on 4/8/17.
 */

public class ProductLazyLoader extends LazyLoader<ProductDomain, ProductDomain.Criteria> {
    List<ProductDomain> _products = new ArrayList<ProductDomain>();

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


        _products.add(prod1);
        _products.add(prod2);
        _products.add(prod3);
        _products.add(prod4);
        _products.add(prod5);
    }

    @Override
    public void LoadAllFields(ProductDomain productDomain, ProductDomain.Criteria criteria) {
        ProductDomain matchedEntity = null;

        for (int index = 0; index < _products.size(); ++index){
            matchedEntity =  _products.get(index);

            if(matchedEntity.Id == criteria.Id)
                break;
        }

        if (matchedEntity == null)
            return;

        productDomain.Id = matchedEntity.Id;
        productDomain.Description = matchedEntity.Description;
    }
}
