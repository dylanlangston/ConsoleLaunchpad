using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using System.Runtime.Loader;

namespace ConsoleLaunchpad
{
    public class MEF : IDisposable
    {
        readonly ContainerConfiguration? Container;
        CompositionHost? Host;

        public MEF(IEnumerable<Assembly> assemblies)
        {
            Container = new ContainerConfiguration().WithAssemblies(assemblies);
            Host = Container.CreateContainer();
        }

        static bool shouldReInit = false;
        public static void AddAssemblies(IEnumerable<string>? assemblies, Action<Exception>? OnError = null)
        {
            if (shouldReInit)
            {
                Importer.Instance.InitMef();
            }
            shouldReInit = true;

            if (!(assemblies?.Any() ?? false)) return;

            Importer.Instance.MEF!.Container!.WithAssemblies(assemblies.Select(a =>
            {
                try
                {
                    return AssemblyLoadContext.Default.LoadFromAssemblyName(AssemblyLoadContext.GetAssemblyName(a));
                }
                catch (Exception er)
                {
                    OnError?.Invoke(er);
                    return null;
                }
            }).Where(a => a != null));
            Importer.Instance.MEF!.Host = Importer.Instance.MEF!.Container!.CreateContainer();
        }

        private sealed class Importer
        {
            public static Importer Instance { get; } = new Importer();
            internal MEF? MEF;

            private Importer()
            {
                DefaultAssemblies = AssemblyLoadContext.Default.Assemblies.ToArray();
                InitMef();
            }

            readonly Assembly[] DefaultAssemblies;
            internal void InitMef()
            {
                MEF?.Dispose();
                MEF = new MEF(DefaultAssemblies);
            }
        }

        public static IEnumerable<T> GetExports<T>() where T : ConsoleLaunchpad.Imports.IImport
        {
            return Importer.Instance.MEF?.Host?.GetExports<T>() ?? throw new NullReferenceException(nameof(Host));
        }

        public static T? GetExport<T>(Func<T, bool> predicate, T? fallback = default) where T : ConsoleLaunchpad.Imports.IImport
        {
            try
            {
                var exports = GetExports<T>();
                return exports.FirstOrDefault(predicate) ?? fallback;
            }
            catch
            {
                return fallback;
            }
        }
        public static T? GetExport<T>(T? fallback = default) where T : ConsoleLaunchpad.Imports.IImport
        {
            try
            {
                var exports = GetExports<T>();
                return exports.FirstOrDefault() ?? fallback;
            }
            catch
            {
                return fallback;
            }
        }

        public static void SatisfyImports(object attributedPart)
        {
            Importer.Instance.MEF?.Host.SatisfyImports(attributedPart);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Host != null)
                {
                    Host.Dispose();
                    Host = null;
                }
            }
        }
    }
}
