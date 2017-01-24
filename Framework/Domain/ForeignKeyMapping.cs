using System.Collections.Generic;

namespace Framework.Domain
{
    public interface IForeignKeyMapping<TParentEntity, TChildEntity>
    {
        IList<TChildEntity> GetChildEntities(TParentEntity parent);
        IList<TChildEntity> RetrieveForeignKeyEntities(TParentEntity parent);
    }

    public abstract class ForeignKeyMapping<TParentEntity, TChildEntity> : IForeignKeyMapping<TParentEntity, TChildEntity>
    {
        public IList<TChildEntity> GetChildEntities(TParentEntity parent)
        {
            return RetrieveForeignKeyEntities(parent);
        }

        public abstract IList<TChildEntity> RetrieveForeignKeyEntities(TParentEntity parent);
    }
}
