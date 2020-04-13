'use strict';

const BaseQueryObject = require('../../Base/BaseQueryObject');
const CustomerFactory = require('../CustomerImplementations/CustomerFactory');

class Criteria{
    constructor(customerId){
        this._customerId = customerId;
    }

    get customerId(){ return this._customerId; }
}

class GetCustomerByIdQuery extends BaseQueryObject{
    constructor(){
        super(Criteria);
    }

    static createCriteria(customerId){
        return new Criteria(customerId);
    }

    //Override base method
    _performSearchOperation(searchInput, cb){
         // Check for both `null` and `undefined`
         if(searchInput == null)
            return cb(null, null);

        const createCustomer = CustomerFactory.factoryImplementation;
        const customerList = [
            createCustomer(1, "Jual dela Cruz"),
            createCustomer(2, "Jane Doe"),
            createCustomer(3, "John Doe")
        ];

        const result = customerList.filter(function(customer){
            return customer.getId() === searchInput.customerId;
        });

        return cb(null, result);
    }
  }

  module.exports = GetCustomerByIdQuery;