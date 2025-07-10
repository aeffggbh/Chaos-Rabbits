using System;
using System.Collections.Generic;

/// <summary>
/// Stores services
/// </summary>
public static class ServiceProvider
{
    private static readonly Dictionary<Type, object> Services = new();

    /// <summary>
    /// Sets a service given a type and a game object.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="service"></param>
    /// <param name="overrideIfFound"></param>
    public static void SetService(Type type, object service,
                                  bool overrideIfFound = false)
    {
        if (!Services.TryAdd(type, service) && overrideIfFound)
            Services[type] = service;
    }
    /// <summary>
    /// Sets a service given a type and a game object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="overrideIfFound"></param>
    public static void SetService<T>(T service, bool overrideIfFound = false)
    {
        if (!Services.TryAdd(typeof(T), service) && overrideIfFound)
            Services[typeof(T)] = service;
    }
    /// <summary>
    /// Returns a service of the type requested
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetService(Type type)  
    {
        return Services.GetValueOrDefault(type);
    }

    /// <summary>
    /// Tries to find the service requested, returns false if it was not found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <returns></returns>
    public static bool TryGetService<T>(out T service) where T : class
    {
        if (Services.TryGetValue(typeof(T), out var serviceObject)
            && serviceObject is T tService)
        {
            service = tService;
            return true;
        }

        service = null;
        return false;
    }
}