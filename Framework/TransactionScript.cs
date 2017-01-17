namespace Framework
{
    public interface ITransactionScript
    {
        IUnitOfWork CreateUnitOfWork();
        void PreExecute();
        void Execute();
        void PostExecute();
    }

    public interface ITransactionScript<TInput, TOutput> : ITransactionScript
    {
        TInput Input { get; set; }
        TOutput Output { get; set; }
    }

    public abstract class TransactionScript<TInput, TOutput> : ITransactionScript<TInput, TOutput>
    {
        public TInput Input { get; set; }
        public TOutput Output { get; set; }

        protected IRepositoryRegistry RepositoryRegistry { get; private set; }
        protected IMapperRegistry MapperRegistry { get; private set; }

        public TransactionScript(IRepositoryRegistry repositoryRegistry, IMapperRegistry mapperRegistry)
        {
            RepositoryRegistry = repositoryRegistry;
            MapperRegistry = mapperRegistry;
        }

        public virtual void PreExecute()
        {
        }

        public abstract void Execute();

        public virtual void PostExecute()
        {
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
