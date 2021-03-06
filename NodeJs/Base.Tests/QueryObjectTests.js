'use strict';

const assert = require('assert');
const BaseQueryObject = require('./../Base/BaseQueryObject');
const GetCustomerByIdQuery = require('./CustomerImplementations/GetCustomerByIdQuery');
const CustomerFactory = require('./CustomerImplementations/CustomerFactory');

describe('GetCustomerByIdQuery', function() {
  describe('#searchInputTypeName', function() {
    it("Get constructed 'searchInputTypeName' value", function(done) {
      const query = new GetCustomerByIdQuery();
      
      query.searchInput = GetCustomerByIdQuery.createCriteria(2);
      
      assert.equal(query.criteriaTypeName, "GetCustomerByIdQuery.Criteria");
      done();
    });
  });
  describe('#execute()', function() {
      it('Execute through promises', function(done) {
        const query = new GetCustomerByIdQuery();
        
        query.searchInput = GetCustomerByIdQuery.createCriteria(2);
        
        query
          .execute()
          .then(result => {
            assert.equal(result.length, 1);
            assert.equal(result[0].getId(), 2);
            assert.equal(result[0].getName(), "Jane Doe");
            done();
          })
          .catch(err => {
            done(err);
          });
      });
    });  
  describe('#execute()', function() {
    it('Execute through callback', function(done) {
      const query = new GetCustomerByIdQuery();
      
      query.searchInput = GetCustomerByIdQuery.createCriteria(2);
      
      assert.equal(query.criteriaTypeName, "GetCustomerByIdQuery.Criteria");
      query.execute((err, result) => {
        assert.equal(result.length, 1);
        assert.equal(result[0].getId(), 2);
        assert.equal(result[0].getName(), "Jane Doe");
      });
      done();
    });
  });
  describe('#getEntityName()', function() {
    it('Execute through callback', function(done) {
      const query = new GetCustomerByIdQuery();
      
      assert.equal(query.entityName, "customer");
      done();
    });
  });
});

  describe('BaseQueryObject', function() {
    describe('#execute()', function() {
      it('Execute through callback(w/error)', function(done) {
        
        const query = new BaseQueryObject(CustomerFactory, null);
        
        query.searchInput = null;
        
        assert.equal(query.criteriaTypeName, "");
        query.execute((err, result) =>{
          // Single equals check for both `null` and `undefined`
          if(err != null)
          {
            assert.equal(err.message, "'_performSearchOperation' overriding is required");
          }

          done();
        });
      });
    });

    describe('#execute()', function() {
      it('Execute through promises(w/error)', function(done) {
        const CustomerFactory = require('./CustomerImplementations/CustomerFactory');
        const query = new BaseQueryObject(CustomerFactory, null);
        
        query.searchInput = null;
        
        assert.equal(query.criteriaTypeName, "");
        query
          .execute()
          .catch(err => {
            // Single equals check for both `null` and `undefined`
            if(err != null)
            {
              assert.equal(err.message, "'_performSearchOperation' overriding is required");
            }            
          });

          done();
      });
    });
  });