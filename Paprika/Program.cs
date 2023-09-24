using Arvy;
using Exy;
using Ria;
using Emi;
using System.Text;

namespace Paprika;

public class Program {
    static void HandlePipelineResult(PipelineContext result) {
        String eventName;

        if (result.ActionMessages.Any()) {
            IList<String> infos = result.ActionMessages
                .Where(resultItem => resultItem.ResponseType == ActionResponseViewModel.Info)
                .Select(resultItem => resultItem.Message)
                .ToList();

            eventName = "Paprika:ResultContainsInfo";
            var args = new EmitterEventArgs(eventName);
            args.Data.Add("ActionMessages", infos);
            GlobalContext.Emitter.Emit(eventName, args);

            IList<String> warns = result.ActionMessages
                .Where(resultItem => resultItem.ResponseType == ActionResponseViewModel.Warning)
                .Select(resultItem => resultItem.Message)
                .ToList();

            eventName = "Paprika:ResultContainsWarn";
            args = new EmitterEventArgs(eventName);
            args.Data.Add("ActionMessages", warns);
            GlobalContext.Emitter.Emit(eventName, args);

            IList<String> errors = result.ActionMessages
                .Where(resultItem => resultItem.ResponseType == ActionResponseViewModel.Error)
                .Select(resultItem => resultItem.Message)
                .ToList();

            if (errors.Any()) {
                eventName = "Paprika:ResultContainsError";
                args = new EmitterEventArgs(eventName);
                args.Data.Add("ActionMessages", errors);
                GlobalContext.Emitter.Emit(eventName, args);

                eventName = "Paprika:PipelineExecutionFailed";
                GlobalContext.Emitter.Emit(eventName, new EmitterEventArgs(eventName));
            }
        }

        eventName = "Paprika:PipelineExecutionSuccess";
        GlobalContext.Emitter.Emit(eventName, new EmitterEventArgs(eventName));
    }

    static String GetHelpText() =>
        new StringBuilder()
            .Append("Usage: Paprika [pipeline name] [config path]")
            .AppendLine()
            .AppendLine("pipeline name:")
            .AppendLine("  choose one pipeline to be ran.")
            .AppendLine()
            .AppendLine("config paths:")
            .AppendLine("  normally is `.\\paprika.config.xml`.")
            .ToString();

    public static void Main(String[] args) {
        try {
            Boolean noArgs = args == null || !args.Any();
            Boolean firstArgsIsHelp = args[0] == "--help";
            if (noArgs || firstArgsIsHelp) {
                Console.WriteLine(GetHelpText());
                return;
            }

            String pipeline = args[0];
            if (args.Length > 1)
                GlobalContext.Initialize(args[1]);
            else
                GlobalContext.Initialize();

            PipelineContext result = GlobalContext.PipelineExecutor.Execute(pipeline);
            HandlePipelineResult(result);
        }
        catch (UnintendedBehaviorException ubex) {
            String eventName = "Paprika:ExceptionThrown";
            var eargs = new EmitterEventArgs(eventName);
            eargs.Data.Add("ExceptionMessage", ubex.Message);
            GlobalContext.Emitter.Emit(eventName, eargs);
        }
        catch (Exception ex) {
            String eventName = "Paprika:ExceptionThrown";
            var eargs = new EmitterEventArgs(eventName);
            eargs.Data.Add("ExceptionMessage", ex.GetExceptionMessage());
            GlobalContext.Emitter.Emit(eventName, eargs);
        }

        // NOTE: this exe intended to be ran in Windows Task Scheduler
#if DEBUG
        Console.ReadLine();
#endif
    }
}