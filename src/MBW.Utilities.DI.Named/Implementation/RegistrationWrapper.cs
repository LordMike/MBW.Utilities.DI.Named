using System;

namespace MBW.Utilities.DI.Named.Implementation;

internal class RegistrationWrapper : IDisposable
{
    public Func<IServiceProvider, object> Factory { private get; set; }

    private bool _created;
    private object _instance;

    public T GetInstance<T>(IServiceProvider serviceProvider)
    {
        if (!_created)
        {
            _instance = (T)Factory(serviceProvider);
            _created = true;
        }

        return (T)_instance;
    }

    public void Dispose()
    {
        (_instance as IDisposable)?.Dispose();
    }
}