'use strict';

const BaseFactory = require('./BaseFactory');

class BaseMapper{
    constructor(factory){
        BaseFactory.validateFactory(factory);

        this._factory = factory;
    }

    /*Abstract methods*/    
    _concreteUpdate(entity, resultCallback){
        throw new Error("'_concreteUpdate' overriding is required"); 
    }

    _concreteInsert(entity, resultCallback){
        throw new Error("'_concreteInsert' overriding is required"); 
    }

    _concreteDelete(entity, resultCallback){
        throw new Error("'_concreteDelete' overriding is required"); 
    }
    /*****************/

    update(entity, resultCallback){
        _execute(_concreteUpdate, entity, resultCallback);
    }

    insert(entity, resultCallback){
        _execute(_concreteInsert, entity, resultCallback);
    }

    _execute(operation, entity, resultCallback)
    {
        //Exposing callbacks and promises for public facing API's
        return new Promise((resolve, reject) =>{
            process.nextTick(() => {    
                const fnError = (error, resultCallback) => {
                    if(resultCallback){
                        resultCallback(error);
                    }

                    reject(error);
                };

                if(typeof operation === 'function'){
                    try{
                        operation(entity, (error, result) =>{
                            // Single equals check for both `null` and `undefined`
                            if(error != null)
                                return fnError(error, resultCallback);
                            
                            if(resultCallback)
                                resultCallback(null, result);
    
                            resolve(result);
                        });
                    }
                    catch(error){
                        return fnError(error, resultCallback);
                    }
                }  
            });
        });
    }
    
    delete(entity, resultCallback){
        _execute(_concreteDelete, entity, resultCallback);
    }    
}

module.exports = BaseMapper;