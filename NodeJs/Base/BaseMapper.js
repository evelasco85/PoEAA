'use strict';

const BaseFactory = require('./BaseFactory');

class BaseMapper{
    constructor(factory){
        BaseFactory.validateFactory(factory);

        this._factory = factory;
    }

    /*Abstract methods*/    
    _concreteUpdate(searchInput, resultCallback){
        throw new Error("'_concreteUpdate' overriding is required"); 
    }

    _concreteInsert(searchInput, resultCallback){
        throw new Error("'_concreteInsert' overriding is required"); 
    }

    _concreteDelete(searchInput, resultCallback){
        throw new Error("'_concreteDelete' overriding is required"); 
    }
    /*****************/

    update(entity, resultCallback)
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

                if(typeof this._concreteUpdate === 'function'){
                    try{
                        this._concreteUpdate(this._searchInput, (error, result) =>{
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
}

module.exports = BaseMapper;