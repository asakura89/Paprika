using Emi;
using Reflx;
using Ria;

namespace Paprika;

public static class GlobalContext {
    public static IEmitter Emitter { get; set; }

    public static IPipelineExecutor PipelineExecutor { get; set; }

    public static void Initialize() {
        DynamicLoadAssemblies();

        ITypeAndAssemblyParser typeNAsmParser = TypeAndAssemblyParser.Instance;
        String paprikaConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "paprika.config.xml");
        Emitter = new XmlConfigEmitterLoader(typeNAsmParser, paprikaConfig).Load();
        PipelineExecutor = new XmlConfigPipelineLoader(paprikaConfig).Load();
    }

    static void DynamicLoadAssemblies() {
        IDefaultAssemblyResolver asmResolver = new DefaultAssemblyResolver();
        AppDomain.CurrentDomain.AssemblyResolve += asmResolver.Resolve;

        IAssemblyLoader asmLoader = new AssemblyLoader();
        asmLoader.LoadFromPath(AppDomain.CurrentDomain.BaseDirectory);
    }
}