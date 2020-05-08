'use strict';

const BaseFactory = require('./BaseFactory');
const TypeCheck = require('./Services/TypeCheckService');

class BaseQueryObject{
    constructor(factory, criteriaType){
        BaseFactory.validateFactory(factory);

        this._className = this.constructor.name;
        this._factory = factory;
        this._criteriaType = criteriaType;
    }

    /*Properties*/
    get entityName() { return this._factory.entityName; }

    get criteriaTypeName(){
        if((this._className) && (this._criteriaType) && (this._criteriaType.name))
            return `${this._className}.${this._criteriaType.name}`;
        else
            return '';
        }

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
                if(!TypeCheck.isFunction(this._performSearchOperation))
                    resolve(null);

                const fnError = (error, resultCallback) => {
                    if(resultCallback){
                        resultCallback(error);
                    }

                    reject(error);
                };

                try{
                    if((this._searchInput != null) && (!TypeCheck.checkType(this._searchInput, this._criteriaType)))
                        throw new Error('"searchInput" is not a valid type'); 
                    
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
            });
        });
    }
}

module.exports = BaseQueryObject;