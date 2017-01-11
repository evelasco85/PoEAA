using System;
using Framework.Domain;

namespace Framework.Data_Manipulation
{
    public interface IBaseMapper
    {
        void Update(object entity);
        void Insert(object entity);
        void Delete(object entity);
    }

    public interface IBaseMapper<TEntity> : IBaseMapper
        where TEntity: IDomainObject
    {
        TEntity CreateEntity();
        void Update(TEntity entity);
        void Insert(TEntity entity);
        void Delete(TEntity entity);
    }

    public abstract class BaseMapper<TEntity> : IBaseMapper<TEntity>
        where TEntity : IDomainObject
    {
        public TEntity CreateEntity()
        {
            TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] {this});

            return entity;
        }

        public abstract void Update(TEntity entity);
        public abstract void Insert(TEntity entity);
        public abstract void Delete(TEntity entity);

        public void Update(object entity)
        {
            TEntity instance = (TEntity) entity;

            Update(instance);
        }

        public void Insert(object entity)
        {
            TEntity instance = (TEntity)entity;

            Insert(instance);
        }

        public void Delete(object entity)
        {
            TEntity instance = (TEntity)entity;

            Delete(instance);
        }
    }
}
