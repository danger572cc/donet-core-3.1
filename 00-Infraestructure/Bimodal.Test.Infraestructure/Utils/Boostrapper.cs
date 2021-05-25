using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bimodal.Test.Infraestructure.Utils
{
    public static class Bootstrapper
    {
        public static void Bootstrap(
            IServiceCollection serviceCollection, 
            IConfiguration configuration, 
            Assembly mainAssembly = null) 
        {
            PreLoadAssemblies(mainAssembly);
            LoadModules(serviceCollection, configuration);
        }

        #region private methods
        private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly
                    .GetTypes()
                    .Where(t => t.FullName != null && t.FullName.StartsWith("Bimodal"));
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types
                    .Where(t => t != null)
                    .Where(t => t.FullName != null && t.FullName.StartsWith("Bimodal"));
            }
        }

        private static void LoadModules(IServiceCollection services, IConfiguration configuration)
        {
            /*services.Scan(s => s
                    .FromExecutingAssembly()
                    .AddClasses()
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());*/
            var types = GetTypes();
            var handlers = (from t in types
                            let attribute = t.GetCustomAttribute<OnStartUpAttribute>()
                            where attribute != null
                            orderby attribute.Order, attribute.Module
                            select t)
                .ToList();
            foreach (var classType in handlers)
            {
                Activator.CreateInstance(classType, services, configuration);
            }
        }

        private static List<Type> GetTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith("Bimodal"))
                .SelectMany(a => a.GetLoadableTypes())
                .AsParallel()
                .ToList();
        }

        private static void LoadAssembliesInPath(Assembly mainAssembly, string path)
        {
            var assembliesInDomain = AppDomain.CurrentDomain.GetAssemblies();
            var allAssemblies = (from a in mainAssembly.GetReferencedAssemblies().AsParallel()
                                 where a.FullName.StartsWith("Bimodal")
                                 select a).ToList();

            if (Directory.Exists(path))
            {
                foreach (string dll in Directory.GetFiles(path, "Bimodal*.dll"))
                {
                    var assemblyInfo = AssemblyName.GetAssemblyName(dll);
                    var fileName = Path.GetFileNameWithoutExtension(dll);

                    if (!assembliesInDomain.Any(a => string.Equals(a.FullName, assemblyInfo.FullName))
                        && allAssemblies.All(a => a.Name != fileName)
                        && mainAssembly.GetName().Name != fileName)
                    {
                        Assembly.LoadFile(dll);
                    }
                }
            }
        }

        private static void PreLoadAssemblies(Assembly mainAssembly)
        {
            LoadAssembliesInPath(mainAssembly, Path.GetDirectoryName(mainAssembly.Location));
            LoadAssembliesInPath(mainAssembly, AppDomain.CurrentDomain.BaseDirectory);
            LoadAssembliesInPath(mainAssembly, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));
        }
        #endregion
    }
}
