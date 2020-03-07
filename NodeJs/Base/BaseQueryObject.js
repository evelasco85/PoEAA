'use strict';

class BaseQueryObject{
    constructor(searchInputType){
        const subclassName = this.constructor.name;
        const searchInputTypeName = searchInputType.name;

        this._searchInputTypeName = `${subclassName}.${searchInputTypeName}`;
    }

    searchInputTypeName(){ return this._searchInputTypeName; }

    get searchInput(){ return this._searchInput; }

    set searchInput(searchInput){
        this._searchInput = searchInput;
    }

    //Abstract method
    _performSearchOperation(searchInput, cb){
        throw new Error("'_performSearchOperation' overriding is required"); 
    }

    execute(cb){
        //Exposing callbacks and promises in public API's
        return new Promise((resolve, reject) =>{
            process.nextTick(() => {               
                if(typeof this._performSearchOperation === 'function'){
                    this._performSearchOperation(this._searchInput, (error, result) =>{
                        if(error){
                            if(cb){
                                cb(error);
                            }

                            return reject(error);
                        }
                        
                        if(cb){
                            cb(null, result);
                        }

                        resolve(result);
                    });
                }  
            });
        });
    }
}

module.exports = BaseQueryObject;
