using System;
using Framework.Data_Manipulation;

namespace Framework.Domain
{
    public interface IDomainObject
    {
        Guid SystemId { get; set; }
        IBaseMapper Mapper { get; set; }
    }

    public class DomainObject : IDomainObject
    {
        private IBaseMapper _mapper;
        private Guid _systemId;

        public Guid SystemId
        {
            get { return _systemId; }
            set { _systemId = value; }
        }

        public IBaseMapper Mapper
        {
            get { return _mapper; }
            set { _mapper = value; }
        }

        //IMemento fields;
        //Identity fields;

        //public DomainObject(IBaseMapper mapper, Guid systemId)
        //{
        //    _mapper = mapper;
        //    _systemId = systemId;
        //}

        /*
         * void RegisterFields<TField>(Expression<Func<bool, TField>> field)
         * {
         * fields.add(field.name, ref field);
         * }
         */
    }
}
