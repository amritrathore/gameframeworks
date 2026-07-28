using System;

public interface ISaveProvider
{
    void Save(string key, object data, Type type);

    object Load(string key, Type type);

    bool Exists(string key);

    void Delete(string key);

    void DeleteAll();
}