'use strict';

const GetCustomerByIdQuery = require('./QueryObjects/GetCustomerByIdQuery');
const query = new GetCustomerByIdQuery();

query.searchInput = GetCustomerByIdQuery.createCriteria(2);

query.execute((err, result) =>{
    console.log("hello world");
});