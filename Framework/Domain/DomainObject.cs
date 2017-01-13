using System;
using System.Diagnostics;
using Framework.Data_Manipulation;

namespace Framework.Domain
{
    public interface ISystemManipulation
    {
        long GetTicksUpdated();
        void MarkAsClean();
    }

    public interface IDomainObject
    {
        Guid SystemId { get; set; }
        IBaseMapper Mapper { get; set; }
        DomainObjectState GetCurrentState();
        void MarkAsDirty();
        void MarkForDeletion();
    }

    public enum DomainObjectState
    {
        Manually_Created = 0,
        Clean = 1,
        Dirty = 2,
        For_DataSource_Deletion = 3
    }

    public class DomainObject : IDomainObject, ISystemManipulation
    {
        public Guid SystemId { get; set; }

        public IBaseMapper Mapper { get; set; }

        private DomainObjectState _state;
        private long _ticksUpdated;

        //IMemento fields;
        //Identity fields;

        public DomainObject()
        {
            SetState(DomainObjectState.Manually_Created);
        }

        public DomainObjectState GetCurrentState()
        {
            return _state;
        }

        void SetState(DomainObjectState state)
        {
            //Stopwatch stopwatch = Stopwatch.StartNew();

            _state = state;
            //_ticksUpdated = DateTime.UtcNow.AddTicks(stopwatch.Elapsed.Ticks).Ticks;
            _ticksUpdated = DateTime.UtcNow.Ticks;

            //stopwatch.Stop();
        }

        //If invoked, 'Update' operation will be applied from the associated mapper
        public void MarkAsDirty()
        {
            SetState(DomainObjectState.Dirty);
        }

        //Exclude from data source updates
        public void MarkAsClean()
        {
            SetState(DomainObjectState.Clean);
        }

        //Deletion takes precedence over all modifiers
        public void MarkForDeletion()
        {
            SetState(DomainObjectState.For_DataSource_Deletion);
        }

        public long GetTicksUpdated()
        {
            return _ticksUpdated;
        }

        /*
         * void RegisterFields<TField>(Expression<Func<bool, TField>> field)
         * {
         * fields.add(field.name, ref field);
         * }
         */

       
    }
}
