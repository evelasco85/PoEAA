package com;

import com.DataManipulation.BaseMapperInterfaces.IBaseMapper;
import com.LazyLoad.LazyLoadList;
import com.LazyLoad.LazyLoader;

import java.util.List;

import com.LazyLoad.ProductDomain;
import com.LazyLoad.ProductLazyLoader;

import static org.junit.Assert.assertEquals;

/**
 * Created by aiko on 4/8/17.
 */

public class LazyLoadTests {
    /****************************/
    //
    //      PLEASE READ 'LazyLoad_README.txt'
    //
    /****************************/
    @org.junit.Test
    public void TestLazyLoadProducts()
    {
        LazyLoader<ProductDomain, ProductDomain.Criteria> loader = new ProductLazyLoader();
        List<ProductDomain> list = new LazyLoadList<ProductDomain, ProductDomain.Criteria>(loader);
        IBaseMapper mapper = null;

        list.add(new ProductDomain(mapper, new ProductDomain.Criteria(2)));
        list.add(new ProductDomain(mapper, new ProductDomain.Criteria(4)));
        list.add(new ProductDomain(mapper, new ProductDomain.Criteria(5)));

        assertEquals("Product two", list.get(0).Description);
        assertEquals("Product four", list.get(1).Description);
        assertEquals("Product five", list.get(2).Description);
    }
}
