using System.Xml;
using AppSea;
using Eksmaru;
using Emi;
using Reflx;
using Ria;
using Varya;

namespace Paprika;

public static class GlobalContext {
    public static IEmitter Emitter { get; set; }

    public static IPipelineExecutor PipelineExecutor { get; set; }

    public static void Initialize() {
        String paprikaConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "paprika.config.xml");
        IList<String> locations = GetAssemblyLocations(paprikaConfigPath);
        DynamicLoadAssemblies(locations);

        ITypeAndAssemblyParser typeNAsmParser = TypeAndAssemblyParser.Instance;
        Emitter = new XmlConfigEmitterLoader(typeNAsmParser, paprikaConfigPath).Load();
        PipelineExecutor = new XmlConfigPipelineLoader(paprikaConfigPath).Load();
    }

    static void DynamicLoadAssemblies(IList<String> locations) {
        IDefaultAssemblyResolver asmResolver = new DefaultAssemblyResolver();
        AppDomain.CurrentDomain.AssemblyResolve += asmResolver.Resolve;

        IAssemblyLoader asmLoader = new AssemblyLoader();
        asmLoader.LoadFromPath(AppDomain.CurrentDomain.BaseDirectory);

        if (locations != null && locations.Any())
            foreach (String location in locations)
                asmLoader.LoadFromPath(location);
    }

    static IList<String> GetAssemblyLocations(String paprikaConfigPath) {
        XmlDocument config = XmlExt.LoadFromPath(paprikaConfigPath);
        XmlNode locationsConfig = config.SelectSingleNode("configuration/locations");
        if (locationsConfig != null) {
            IList<String> locations = locationsConfig
                .SelectNodes("location")
                .Cast<XmlNode>()
                .Select(config => {
                    String pathValue = config.GetAttributeValue("path");
                    if (String.IsNullOrEmpty(pathValue))
                        throw new BadConfigurationException("Wrong Path configuration. '{pathValue}'.");

                    return pathValue.Resolve();
                })
                .ToList();

            return locations;
        }

        return Enumerable
            .Empty<String>()
            .ToList();
    }
}