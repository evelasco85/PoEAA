namespace Framework.Domain
{
    public interface IObjectStateContainer
    {

    }

    public class ObjectStateContainer : IObjectStateContainer
    {
        void Monitor()//object
        {
        }

        void UndoChanges()//All or specific
        {

        }

        void GetModifiedObjects() //All or specific
        {

        }

        void ApplyChangesToContainer()
        {

        }

        void HasChanges()
        {

        }
    }
}
