using Emi;

namespace Paprika;

public class DefaultEventHandler {
    void DisplayAllActionMessages(EmitterEventArgs args) {
        var messages = args.Data["ActionMessages"] as IList<String>;
        foreach (String message in messages)
            Console.WriteLine(message);
    }

    public void OnResultContainsInfo(EmitterEventArgs args) => DisplayAllActionMessages(args);

    public void OnResultContainsWarn(EmitterEventArgs args) => DisplayAllActionMessages(args);

    public void OnResultContainsError(EmitterEventArgs args) => DisplayAllActionMessages(args);

    public void OnPipelineExecutionFailed(EmitterEventArgs args) => Console.WriteLine("Pipeline execution failed.");

    public void OnPipelineExecutionSuccess(EmitterEventArgs args) => Console.WriteLine("Pipeline execution success.");

    public void OnExceptionThrown(EmitterEventArgs args) {
        String message = args.Data["ExceptionMessage"].ToString();
        Console.WriteLine(message);
    }
}