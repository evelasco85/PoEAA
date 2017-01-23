using System;

namespace Framework
{
    public interface IIdentityFieldAttribute
    { }

    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityFieldAttribute : Attribute, IIdentityFieldAttribute
    {
    }
}
