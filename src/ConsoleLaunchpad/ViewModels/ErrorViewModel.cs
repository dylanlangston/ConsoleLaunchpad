using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ConsoleLaunchpad.ViewModels
{
    public class ErrorViewModelBase : ViewModelBase
    {
        public string? ErrorText { get; init; }
        public string? StackTraceText { get; init; }
    }

    public class ErrorViewModel : ErrorViewModelBase
    {
#if DEBUG
        public ErrorViewModel()
        {
            Exception exception = new("Foobar");
            Exception = exception;
            ErrorText = exception.ToString();
            StackTrace = new StackTrace(true);
            StackTraceText = StackTrace?.ToString();
        }
#endif

        public ErrorViewModel(Exception exception)
        {
            Exception = exception;
            ErrorText = exception.GetType().FullName + ": " + exception.Message;
            StackTrace = new StackTrace(exception, true);
            if (StackTrace?.FrameCount > 0)
            {
                StackTraceText = StackTrace!.ToString();
            }
            else
            {
                StackTraceText = "Stack Trace Missing.";
            }
        }
        internal ErrorViewModel(ErrorViewModelBase viewModel)
        {
            ErrorText = viewModel.ErrorText;
            StackTraceText = viewModel.StackTraceText;
        }

        [JsonIgnore]
        public Exception? Exception { get; private init; }
        public new string? ErrorText { get; private init; }
        [JsonIgnore]
        public StackTrace? StackTrace { get; private init; }
        public new string? StackTraceText { get; private init; }
    }
}
