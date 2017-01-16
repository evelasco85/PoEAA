using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public interface IDomainObjectMementoService
    {
        IDomainObjectMemento CreateMemento<TEntity>(TEntity entity)
            where TEntity : IDomainObject;

        void SetMemento<TEntity>(TEntity entity, IDomainObjectMemento memento)
            where TEntity : IDomainObject;
    }

    public class DomainObjectMementoService : IDomainObjectMementoService
    {
        static IDomainObjectMementoService s_instance = new DomainObjectMementoService();

        private DomainObjectMementoService()
        {
        }

        public static IDomainObjectMementoService GetInstance()
        {
            return s_instance;
        }

        public IDomainObjectMemento CreateMemento<TEntity>(TEntity entity)
            where TEntity : IDomainObject
        {
            DomainObjectMemento memento = new DomainObjectMemento();

            return memento;
        }

        public void SetMemento<TEntity>(TEntity entity, IDomainObjectMemento memento)
            where TEntity : IDomainObject
        {
        }
    }
}
