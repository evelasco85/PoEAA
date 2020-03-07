'use strict';

const BaseQueryObject = require('./../../Base/BaseQueryObject');

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

        const customerList = [
            {id: 1, name: "Jual dela Cruz"},
            {id: 2, name: "Jane Doe"},
            {id: 3, name: "John Doe"}
        ];

        const result = customerList.filter(function(customer){
            return customer.id === searchInput.customerId;
        });

        return cb(null, result);
    }
  }

  module.exports = GetCustomerByIdQuery;