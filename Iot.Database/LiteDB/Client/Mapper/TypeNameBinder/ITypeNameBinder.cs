using System;

namespace Iot.Database
{
    public interface ITypeNameBinder
    {
        string GetName(Type type);
        Type GetType(string name);
    }
}