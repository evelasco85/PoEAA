using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data_Manipulation;

namespace Framework.Domain
{
    public interface IDomainObject
    {
    }

    public class DomainObject : IDomainObject
    {
        private IBaseMapper _mapper;

        //IMemento fields;

        public DomainObject(IBaseMapper mapper)
        {
            _mapper = mapper;
        }

        /*
         * void RegisterFields<TField>(Expression<Func<bool, TField>> field)
         * {
         * fields.add(field.name, ref field);
         * }
         */
    }
}
