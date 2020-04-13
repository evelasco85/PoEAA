function createCustomer(id, name){
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

module.exports = {
     factoryName: "customerFactory",
     factoryImplementation: createCustomer
    };
