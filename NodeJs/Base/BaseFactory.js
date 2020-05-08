'use strict';
const TypeCheck = require('./Services/TypeCheckService');

class BaseFactory{
    constructor(entityName){
        this._entityName = entityName;
    }

    /*Properties*/    
    get entityName(){ return this._entityName; }
    /************/

    /*Static member implementations*/    
    static validateFactory(factory){
        if(factory == null)
            throw new Error("'factory' argument is required"); 

        if(!TypeCheck.isInstanceOf(factory, BaseFactory))
            throw new Error("'factory' instance requires 'BaseFactory' base type");
    }
    /*******************************/
}

module.exports = BaseFactory;