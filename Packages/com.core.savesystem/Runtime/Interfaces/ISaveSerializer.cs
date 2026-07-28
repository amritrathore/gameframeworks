using System;

namespace Core.SaveSystem
{
    public interface ISaveSerializer
    {
        string Serialize(object data);

        object Deserialize(string json, Type type);
    }
}