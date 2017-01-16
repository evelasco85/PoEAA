using System;
using Framework.Domain;

namespace Framework
{
    public interface IUnitOfWorkEntityWrapper
    {
        IDomainObject EntityObject { get; }
        Guid SystemId { get; }
        InstantiationType Instantiation { get; }
        bool SetForDataSourceDeletion { get; set; }
        string OriginalHashCode { get; }
        UnitOfWorkAction GetExpectedAction();
        long GetTicksUpdated();
        bool HasChanges();
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

        private bool _setForDataSourceDeletion = false;
        private readonly TEntity _entity;
        private readonly string _originalHashCode;

        IDomainObjectMementoService _mementoService = DomainObjectMementoService.GetInstance();

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

        public bool SetForDataSourceDeletion
        {
            get { return _setForDataSourceDeletion; }
            set
            {
                if (_entity.Instantiation != InstantiationType.Loaded)
                    throw new InvalidOperationException("");

                _setForDataSourceDeletion = value;
            }
        }

        public InstantiationType Instantiation
        {
            get { return _entity.Instantiation; }
        }

        public string OriginalHashCode
        {
            get { return _originalHashCode; }
        }

        public UnitOfWorkEntityWrapper(TEntity entity)
        {
            IDomainObjectMemento memento = _mementoService.CreateMemento(entity);

            _entity = entity;
            _originalHashCode = memento.HashCode;
            _ticksUpdated = GetTick();
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
            if(_entity.Instantiation == InstantiationType.New )
                return UnitOfWorkAction.Insert;

            return UnitOfWorkAction.None;
        }

        public bool HasChanges()
        {
            IDomainObjectMemento memento = _mementoService.CreateMemento(_entity);

            return (_originalHashCode != memento.HashCode);
        }
    }
}
