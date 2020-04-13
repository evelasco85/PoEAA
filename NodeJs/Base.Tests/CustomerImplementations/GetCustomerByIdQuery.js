'use strict';

const BaseQueryObject = require('../../Base/BaseQueryObject');
const CustomerFactory = require('../CustomerImplementations/CustomerFactory');
const createCustomer = CustomerFactory.createCustomer;

class Criteria{
    constructor(customerId){
        this._customerId = customerId;
    }

    get customerId(){ return this._customerId; }
}

class GetCustomerByIdQuery extends BaseQueryObject{
    constructor(){
        super(CustomerFactory, Criteria);
    }

    static createCriteria(customerId){
        return new Criteria(customerId);
    }

    /*Override base method*/    
    _performSearchOperation(searchInput, resultCallback){                               //Will be invoked by base class
         if(searchInput == null)                                            // Check for both `null` and `undefined`
            return resultCallback(null, null);

        const customerList = [
            createCustomer(1, "Jual dela Cruz"),
            createCustomer(2, "Jane Doe"),
            createCustomer(3, "John Doe")
        ];

        const result = customerList.filter(function(customer){
            return customer.getId() === searchInput.customerId;
        });

        return resultCallback(null, result);
    }
    /**********************/
  }

  module.exports = GetCustomerByIdQuery;