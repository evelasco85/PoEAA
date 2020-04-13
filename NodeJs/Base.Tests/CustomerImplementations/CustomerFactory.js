'use strict';

const BaseFactory = require('../../Base/BaseFactory');

class CustomerFactory extends BaseFactory{
    constructor(){
        super("customer")
    }

    createCustomer(id, name){
        const _properties = {};
    
        const customer = {
            setName: name =>{
                _properties.name = name;
            },
            getName: () => {
                return _properties.name;
            },
    
            setId: id => {
                _properties.id = id;
            },
            getId: () => {
                return _properties.id;
            }
        };
    
        customer.setId(id);
        customer.setName(name);
    
        return customer;
    }
}

const factory = new CustomerFactory();

module.exports = factory;
