'use strict';

class BaseQueryObject{
    constructor(searchInputType){
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
    get searchInputTypeName(){ return this._searchInputTypeName; }

    get searchInput(){ return this._searchInput; }

    set searchInput(searchInput){
        this._searchInput = searchInput;
    }
    /************/

    /*Abstract method*/    
    _performSearchOperation(searchInput, cb){
        throw new Error("'_performSearchOperation' overriding is required"); 
    }
    /*****************/

    execute(cb){
        //Exposing callbacks and promises in public API's
        return new Promise((resolve, reject) =>{
            process.nextTick(() => {    
                const fnError = (error, cb) => {
                    if(cb){
                        cb(error);
                    }

                    reject(error);
                };

                if(typeof this._performSearchOperation === 'function'){
                    try{
                        this._performSearchOperation(this._searchInput, (error, result) =>{
                            // Single equals check for both `null` and `undefined`
                            if(error != null)
                                return fnError(error, cb);
                            
                            if(cb)
                                cb(null, result);
    
                            resolve(result);
                        });
                    }
                    catch(error){
                        return fnError(error, cb);
                    }
                }  
            });
        });
    }
}

module.exports = BaseQueryObject;
