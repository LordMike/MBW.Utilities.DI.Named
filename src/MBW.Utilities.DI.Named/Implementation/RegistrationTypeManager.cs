using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MBW.Utilities.DI.Named.Implementation;

internal static class RegistrationTypeManager
{
    public const string AssemblyName = "NamedDI.DynamicTypes";
    
    private const string MainModuleName = "MainModule";
    private static object _lockObj = new();
    private static readonly ModuleBuilder _moduleBuilder;

    /// <summary>
    /// Unique type+name => marker-type registrations
    /// </summary>
    private static readonly Dictionary<(Type serviceType, string name), Type> _registrationTypes = new();

    /// <summary>
    /// Map service types to names, for getting all named services of type T.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, (string name, Type registrationType)[]> _registrationTypesByServiceType = new();

    static RegistrationTypeManager()
    {
        AssemblyName an = new AssemblyName(AssemblyName);
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
        _moduleBuilder = assemblyBuilder.DefineDynamicModule(MainModuleName);
    }

    public static Type GetRegistrationWrapperType(Type serviceType, string name, bool allowCreate)
    {
        (Type serviceType, string name) key = (serviceType, name);
        Type type;

        if (!allowCreate)
        {
            _registrationTypes.TryGetValue(key, out type);
            return type;
        }

        if (_registrationTypes.TryGetValue(key, out type))
            return type;

        // Only acquire a lock if the type we need _might_ need to be created. If it is, we need the lock to ensure we
        // don't create two types for the same name, as that is a violation.
        lock (_lockObj)
        {
            if (_registrationTypes.TryGetValue(key, out type))
                return type;

            // Create and record new type
            string typeName = $"{serviceType.FullName}__{name}";

            TypeBuilder typeBuilder = _moduleBuilder.DefineType(typeName,
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, null);

            typeBuilder.SetParent(typeof(RegistrationWrapper));

            Type registrationWrapperType = typeBuilder.CreateTypeInfo().AsType();
            _registrationTypes.Add(key, registrationWrapperType);

            if (_registrationTypesByServiceType.TryGetValue(serviceType, out (string name, Type registrationType)[] existingList))
            {
                // Note: We create new lists here to be able to return immutable lists when queried
                (string name, Type registrationType)[] newList = new (string name, Type registrationType)[existingList.Length + 1];
                existingList.CopyTo(newList, 0);

                newList[newList.Length - 1] = (name, registrationWrapperType);

                _registrationTypesByServiceType[serviceType] = newList;
            }
            else
            {
                // Create an entirely new list
                (string name, Type registrationType)[] newList = new (string name, Type registrationType)[1];
                newList[0] = (name, registrationWrapperType);

                _registrationTypesByServiceType[serviceType] = newList;
            }

            return registrationWrapperType;
        }
    }

    public static IEnumerable<(string name, Type registrationType)> GetRegistrationTypesAndNames(Type serviceType)
    {
        if (!_registrationTypesByServiceType.TryGetValue(serviceType, out var lst))
            return Enumerable.Empty<(string, Type)>();

        return lst;
    }
}