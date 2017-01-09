using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using MMU.BoerseDownloader.Common.Interfaces;

namespace MMU.BoerseDownloader.Common.Singletons
{
    public static class UnityContainerSingleton
    {
        public static IUnityContainer Instance { get; private set; }

        public static void Initialize()
        {
            var unityContainer = new UnityContainer();
            var allUnityInitializers = GetAllUnityInitializers();
            allUnityInitializers.ForEach(f => f.InitializeContainer(unityContainer));
            Instance = unityContainer;
        }

        private static IEnumerable<IUnityInitializer> CreateInitializerInstancesFromAssembly(Assembly assembly)
        {
            var result = new List<IUnityInitializer>();
            var initializerTypes = assembly.GetTypes().Where(typeof(IUnityInitializer).IsAssignableFrom).Where(t => !t.IsInterface && !t.IsAbstract);

            foreach (var it in initializerTypes)
            {
                var initializerInstance = (IUnityInitializer)Activator.CreateInstance(it);
                result.Add(initializerInstance);
            }

            return result;
        }

        private static IEnumerable<IUnityInitializer> GetAllUnityInitializers()
        {
            var result = new List<IUnityInitializer>();
            var applicationAssemblies = GetApplicationAssemblies();

            foreach (var appAssembly in applicationAssemblies)
            {
                var initializers = CreateInitializerInstancesFromAssembly(appAssembly);
                result.AddRange(initializers);
            }

            return result;
        }

        private static IEnumerable<Assembly> GetApplicationAssemblies()
        {
            var consideredFileExtensions = new[]
            {
                ".dll",
                ".exe"
            };

            var result = new List<Assembly>();
            var namespaceStartingPart = GetNamespaceStartingPart();

            var assemblyPath = GetPath();
            IEnumerable<string> assemblyFiles = Directory.GetFiles(assemblyPath);

            var fileInfos = assemblyFiles.Select(f => new FileInfo(f));
            fileInfos = fileInfos.Where(f => f.Name.StartsWith(namespaceStartingPart) && consideredFileExtensions.Contains(f.Extension.ToLower()));

            foreach (var fi in fileInfos)
            {
                var assembly = Assembly.LoadFile(fi.FullName);
                result.Add(assembly);
            }

            return result;
        }

        private static string GetNamespaceStartingPart()
        {
            var fullNamespace = Assembly.GetExecutingAssembly().FullName;
            var splittedNamespace = fullNamespace.Split('.');

            var result = string.Concat(splittedNamespace[0], ".", splittedNamespace[1]);
            return result;
        }

        private static string GetPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string result = Uri.UnescapeDataString(uri.Path);
            result = Path.GetDirectoryName(result);

            return result;
        }
    }
}