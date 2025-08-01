using System;
using System.Collections.Generic;
/// <summary>
/// Contains data for the player
/// </summary>
public static class PlayerPreservedData
{
    private static readonly Dictionary<Type, object> PreservedObjects = new();

    private static bool _acceptingData = true;

    public static void SaveData<T>(T data) where T : IData
    {
        if (_acceptingData)
            PreservedObjects[typeof(T)] = data;
        else
            _acceptingData = true;
    }

    public static T RetrieveData<T>() where T : IData
    {
        if (PreservedObjects.TryGetValue(typeof(T), out var data))
            return (T)data;

        return default;
    }

    public static bool IsDataIn<T>() where T : IData
    {
        return PreservedObjects.ContainsKey(typeof(T));
    }

    public static bool IsEmpty()
    {
        return (PreservedObjects.Count == 0);
    }

    public static void Reset()
    {
        PreservedObjects.Clear();
        _acceptingData = false;
    }
}
