'use strict';

module.exports.checkType = (instance, expectedType) => {
    return (instance != null) &&
        (instance.constructor != null) &&
        (expectedType != null) &&
        (instance.constructor.name === expectedType.name);
};

module.exports.isInstanceOf = (instance, expectedType) => {
    return (instance instanceof expectedType);
};

module.exports.isFunction = (member) =>{
    return (typeof member === 'function');
};