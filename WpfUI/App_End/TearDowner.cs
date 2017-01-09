using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace MMU.BoerseDownloader.WpfUI.App_End
{
    internal static class TearDowner
    {
        private static readonly object _lock = new object();
        private static bool _tornDown = false;

        internal static void Teardown()
        {
            if (!_tornDown)
            {
                lock (_lock)
                {
                    if (!_tornDown)
                    {
                        var disposableType = typeof(IDisposable);
                        var containerControlledLifeTimeManagerType = typeof(ContainerControlledLifetimeManager);
                        var disposables = Common.Singletons.UnityContainerSingleton.Instance.Registrations.Where(cm => disposableType.IsAssignableFrom(cm.RegisteredType) && cm.LifetimeManagerType == containerControlledLifeTimeManagerType).ToList();

                        foreach (var disp in disposables)
                        {
                            var disposableObjects = new List<IDisposable>();
                            var multiRegistered = Common.Singletons.UnityContainerSingleton.Instance.ResolveAll(disp.RegisteredType).Cast<IDisposable>().ToList();
                            disposableObjects.AddRange(multiRegistered);

                            var singleRegistered = Common.Singletons.UnityContainerSingleton.Instance.Resolve(disp.RegisteredType) as IDisposable;
                            if (singleRegistered != null)
                            {
                                disposableObjects.Add(singleRegistered);
                            }

                            disposableObjects.ForEach(f => f.Dispose());
                        }

                        _tornDown = true;
                    }
                }
            }
        }
    }
}