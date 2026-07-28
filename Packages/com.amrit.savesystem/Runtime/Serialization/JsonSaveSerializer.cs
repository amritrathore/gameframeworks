using System;
using UnityEngine;

namespace Amrit.SaveSystem
{
    public sealed class JsonSaveSerializer : ISaveSerializer
    {
        public string Serialize(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return JsonUtility.ToJson(data, true);
        }

        public object Deserialize(
            string serializedData,
            Type type)
        {
            if (string.IsNullOrWhiteSpace(serializedData))
            {
                return null;
            }

            return JsonUtility.FromJson(serializedData, type);
        }
    }
}