using System;
using System.Collections.Generic;
using Framework.Domain;

namespace Framework.Data_Manipulation
{
    public interface IBaseCriteria<TEntity>
       where TEntity : IDomainObject
    {
        IList<TEntity> Execute();
    }

    public interface IBaseCriteria<TEntity, TInput, TOperationResult> : IBaseCriteria<TEntity>
        where TEntity : IDomainObject
    {
        TInput Input { get; set; }
        TOperationResult PerformOperation(TInput Input);
        IList<TEntity> ConstructOutput(TOperationResult result, IBaseMapper_Instantiator<TEntity> mapper);
        void ChainOperationResultToDownstreamCriteria(
            IBaseCriteria<TEntity, TOperationResult, IList<TEntity>> downstreamCriteria);
    }

    //Builder pattern
    //Data source, table, and stored procedure agnostic
    //1 criterion = 1 lookup operation
    //conditions
    public abstract class BaseCriteria<TEntity, TInput, TOperationResult> :
        IBaseCriteria<TEntity, TInput, TOperationResult>
        where TEntity : IDomainObject
    {
        private IBaseCriteria<TEntity, TOperationResult, IList<TEntity>> _downstreamCriteria;

        public TInput Input { get; set; }

        public abstract TOperationResult PerformOperation(TInput input);

        public virtual IList<TEntity> ConstructOutput(TOperationResult result, IBaseMapper_Instantiator<TEntity> mapper)
        {
            //Implementation deferred to ultimate processor
            throw new NotImplementedException();
        }

        public void ChainOperationResultToDownstreamCriteria(
            IBaseCriteria<TEntity, TOperationResult, IList<TEntity>> downstreamCriteria)
        {
            _downstreamCriteria = downstreamCriteria;
        }

        public IList<TEntity> Execute()
        {
            TOperationResult operationResult = PerformOperation(Input);

            IList<TEntity> ultimateResult;

            if (_downstreamCriteria != null)
            {
                _downstreamCriteria.Input = operationResult;

                ultimateResult = _downstreamCriteria.Execute();
            }
            else
            {
                IBaseMapper<TEntity> mapper = DataSynchronizationManager.GetInstance().GetMapper<TEntity>();

                ultimateResult = ConstructOutput(operationResult, mapper);
            }

            return ultimateResult;
        }
    }
}
