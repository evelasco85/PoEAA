'use strict';

const assert = require('assert');
const GetCustomerByIdQuery = require('./QueryObjects/GetCustomerByIdQuery');
const query = new GetCustomerByIdQuery();

query.searchInput = GetCustomerByIdQuery.createCriteria(2);

query.execute((err, result) =>{
    console.log("hello world");
});

describe('GetCustomerByIdQuery', function() {
    describe('#execute()', function() {
      it('should return -1 when the value is not present', function() {
        assert.equal([1, 2, 3].indexOf(4), -1);
      });
    });
  });