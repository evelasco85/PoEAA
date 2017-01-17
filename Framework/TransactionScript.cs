namespace Framework
{
    public interface ITransactionScriptExecution
    {
        void RunScript();
    }

    public interface ITransactionScript : ITransactionScriptExecution
    {
        IUnitOfWork CreateUnitOfWork();
        void PreExecuteBody();
        void ExecutionBody();
        void PostExecuteBody();
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

        protected TransactionScript(IRepositoryRegistry repositoryRegistry, IMapperRegistry mapperRegistry)
        {
            RepositoryRegistry = repositoryRegistry;
            MapperRegistry = mapperRegistry;
        }

        public virtual void PreExecuteBody()
        {
        }

        public abstract void ExecutionBody();

        public virtual void PostExecuteBody()
        {
        }

        public void RunScript()
        {
            PreExecuteBody();
            ExecutionBody();
            PostExecuteBody();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
