using System;
using Emi;

namespace PaprikaTaskExample {
    public class ExampleEventHandler {
        public void OnResultContainsInfo(EmitterEventArgs args) => Console.WriteLine($"{nameof(OnResultContainsInfo)} is called.");

        public void OnResultContainsWarn(EmitterEventArgs args) => Console.WriteLine($"{nameof(OnResultContainsWarn)} is called.");

        public void OnResultContainsError(EmitterEventArgs args) => Console.WriteLine($"{nameof(OnResultContainsError)} is called.");

        public void OnPipelineExecutionFailed(EmitterEventArgs args) => Console.WriteLine($"{nameof(OnPipelineExecutionFailed)} is called.");

        public void OnPipelineExecutionSuccess(EmitterEventArgs args) => Console.WriteLine($"{nameof(OnPipelineExecutionSuccess)} is called.");

        public void OnExceptionThrown(EmitterEventArgs args) => Console.WriteLine($"{nameof(OnExceptionThrown)} is called.");
    }
}
