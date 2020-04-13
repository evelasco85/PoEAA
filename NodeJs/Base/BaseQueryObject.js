'use strict';

const BaseFactory = require('./BaseFactory');

class BaseQueryObject{
    constructor(factory, searchInputType){
        BaseFactory.validateFactory(factory);

        this._factory = factory;

        if((this.constructor.name) && (searchInputType) && (searchInputType.name))
        {
            const concreteClassName = this.constructor.name;
            const typeName = searchInputType.name;

            this._searchInputTypeName = `${concreteClassName}.${typeName}`;
        }
        else
            this._searchInputTypeName = '';
    }

    /*Properties*/
    get entityName() { return this._factory.entityName; }

    get searchInputTypeName(){ return this._searchInputTypeName; }

    get searchInput(){ return this._searchInput; }

    set searchInput(searchInput){
        this._searchInput = searchInput;
    }
    /************/

    /*Abstract method*/
    _performSearchOperation(searchInput, resultCallback){
        throw new Error("'_performSearchOperation' overriding is required"); 
    }
    /*****************/

    execute(resultCallback){
        //Exposing callbacks and promises for public facing API's
        return new Promise((resolve, reject) =>{
            process.nextTick(() => {    
                const fnError = (error, resultCallback) => {
                    if(resultCallback){
                        resultCallback(error);
                    }

                    reject(error);
                };

                if(typeof this._performSearchOperation === 'function'){
                    try{
                        this._performSearchOperation(this._searchInput, (error, result) =>{
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

module.exports = BaseQueryObject;