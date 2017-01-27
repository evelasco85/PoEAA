using System;
using Framework.Domain;

namespace Framework
{
    public interface IUnitOfWorkEntityWrapper
    {
        IDomainObject EntityObject { get; }
        Guid SystemId { get; }
        InstantiationType Instantiation { get; }
        UnitOfWorkAction GetExpectedAction();
        long GetTicksUpdated();
    }

    public interface IUnitOfWorkEntityWrapper<TEntity> : IUnitOfWorkEntityWrapper
        where TEntity : IDomainObject
    {
        TEntity Entity { get; }
    }

    public class UnitOfWorkEntityWrapper<TEntity> : IUnitOfWorkEntityWrapper<TEntity>
        where TEntity : IDomainObject
    {
        private long _ticksUpdated;
        static object s_lock = new object();
        static long s_lastTickReceived;
        static int s_tickOffset;

        private readonly TEntity _entity;
        private readonly UnitOfWorkAction _action;

        public TEntity Entity
        {
            get { return _entity; }
        }

        public IDomainObject EntityObject
        {
            get { return _entity; }
        }

        public Guid SystemId
        {
            get { return _entity.SystemId; }
        }

        public InstantiationType Instantiation
        {
            get { return _entity.Instantiation; }
        }

        public UnitOfWorkEntityWrapper(TEntity entity, UnitOfWorkAction action)
        {
            _entity = entity;
            _ticksUpdated = GetTick();
            _action = action;
        }

        long GetTick()
        {

            long currentTick = DateTime.Now.Ticks;
            long tickValue = 0;

            lock (s_lock)
            {
                if (s_lastTickReceived == currentTick)
                {
                    s_tickOffset += 1;
                    tickValue = currentTick + s_tickOffset;
                }
                else
                {
                    s_tickOffset = 0;
                    s_lastTickReceived = currentTick;
                    tickValue = currentTick;
                }
            }

            return tickValue;
        }

        public long GetTicksUpdated()
        {
            return _ticksUpdated;
        }

        public UnitOfWorkAction GetExpectedAction()
        {
            return _action;
        }
    }
}
