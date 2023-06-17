using NetTemplate.Common.IO.Extensions;
using System.Reflection;

namespace NetTemplate.Common.Reflection.Utils
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Type> GetTypesOfNamespace(string nameSpace,
            Assembly assembly = null, bool includeSubns = false)
        {
            assembly = assembly ?? Assembly.GetEntryAssembly();

            return assembly.GetTypes()
                .Where(type => includeSubns ?
                    type.Namespace == nameSpace || type.Namespace?.StartsWith(nameSpace + ".") == true :
                    type.Namespace == nameSpace);
        }

        public static Assembly[] GetDomainAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public static List<Assembly> GetAllAssemblies(string rootPath = null,
            string searchPattern = "*.dll", IEnumerable<string> excludedRelativeDirPaths = null,
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (string.IsNullOrEmpty(rootPath))
                rootPath = GetEntryAssemblyLocation();
            string rootFolderPath = Path.GetDirectoryName(rootPath);

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .ToDictionary(o => o.FullName);

            List<Assembly> allAssemblies = new List<Assembly>();

            var excludedDirs = excludedRelativeDirPaths?.Select(dirPath => new DirectoryInfo(
                Path.Combine(Directory.GetParent(rootPath).FullName, dirPath))).ToArray() ?? new DirectoryInfo[] { };

            var targetAssemblies = Directory.EnumerateFiles(rootFolderPath, searchPattern, searchOption)
                .Where(file => !excludedDirs.Any(dir => Directory.GetParent(file).IsSubDirectoryOf(dir)))
                .Select(dll => new
                {
                    Dll = dll,
                    Name = SafelyGetAssemblyName(dll)
                }).Where(assObj => assObj.Name != null).ToArray();

            foreach (var assObj in targetAssemblies)
            {
                if (loadedAssemblies.ContainsKey(assObj.Name.FullName))
                {
                    allAssemblies.Add(loadedAssemblies[assObj.Name.FullName]);
                    continue;
                }

                try
                {
                    var assembly = Assembly.Load(assObj.Name);
                    allAssemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                {
                    var assembly = Assembly.LoadFile(assObj.Dll);
                    allAssemblies.Add(assembly);
                }
                catch (FileLoadException) { }
                catch (BadImageFormatException) { }
            }

            return allAssemblies;
        }

        public static IEnumerable<Type> GetAllTypesDefined(Type attributeType, IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(o => o.GetTypes()).Where(type => type.IsDefined(attributeType));
            return types;
        }

        public static IEnumerable<Type> GetAllTypesAssignableTo(Type baseType, IEnumerable<Assembly> assemblies,
            bool includeGeneric = true, bool baseTypeExcluded = true, bool isAbstract = false, bool isInterface = false)
        {
            var types = assemblies.SelectMany(o => o.GetTypes()).Where(type =>
                (baseType.IsAssignableFrom(type)
                    || includeGeneric && type.GetInterfaces().Any(itf => itf.IsGenericType
                        && baseType.IsAssignableFrom(itf.GetGenericTypeDefinition())))
                && (!baseTypeExcluded || type != baseType) && type.IsAbstract == isAbstract
                && type.IsInterface == isInterface);

            return types;
        }

        public static AssemblyName SafelyGetAssemblyName(string assFile)
        {
            try
            {
                return AssemblyName.GetAssemblyName(assFile);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetEntryAssemblyLocation()
        {
            return Assembly.GetEntryAssembly().Location;
        }

        public static IEnumerable<TAttribute> GetAttributesOfMemberOrType<TAttribute>(
            MemberInfo memberInfo, bool inherit = true) where TAttribute : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes<TAttribute>(inherit).ToArray();

            if (attributes.Length == 0)
            {
                attributes = memberInfo.ReflectedType?.GetCustomAttributes<TAttribute>(inherit).ToArray();
            }

            return attributes ?? new TAttribute[0];
        }
    }
}
