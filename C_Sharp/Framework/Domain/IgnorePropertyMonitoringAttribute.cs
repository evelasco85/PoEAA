using System;

namespace Framework.Domain
{
    public interface IIgnorePropertyMonitoringAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyMonitoringAttribute : Attribute, IIgnorePropertyMonitoringAttribute
    {
    }
}
