using System;
using System.IO;
using Emi;
using Ria;
using Reflx;
using Arvy;

namespace PaprikaTaskExample {
    public class Task2 {
        public void Process(TaskExampleContext context) {
            IDefaultAssemblyResolver asmResolver = new DefaultAssemblyResolver();
            AppDomain.CurrentDomain.AssemblyResolve += asmResolver.Resolve;

            IAssemblyLoader asmLoader = new AssemblyLoader();
            asmLoader.LoadFromPath(AppDomain.CurrentDomain.BaseDirectory);

            ITypeAndAssemblyParser typeNAsmParser = TypeAndAssemblyParser.Instance;
            String config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "paprika.config.xml");
            IEmitter emitter = new XmlConfigEmitterLoader(typeNAsmParser, config).Load();

            emitter.Emit("task2:started", new EmitterEventArgs("task2:started"));
            context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Info, $"{nameof(Task2)} - Information from {nameof(Task2)}."));
            context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Info, $"{nameof(Task2)} - More information from {nameof(Task2)}."));
            context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Success, $"{nameof(Task2)} - Ok. let's end it here."));
            emitter.Emit("task2:finished", new EmitterEventArgs("task2:finished"));
        }

        public void OnTask2ProcessStarted(EventArgs args) => Console.WriteLine($"{nameof(Task2)} started.");

        public void OnTask2ProcessFinished(EventArgs args) => Console.WriteLine($"{nameof(Task2)} finished.");
    }
}
