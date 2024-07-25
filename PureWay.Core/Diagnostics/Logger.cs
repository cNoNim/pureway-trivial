using System.Collections.Immutable;

namespace PureWay.Core.Diagnostics;

internal sealed class Logger<TModule>(ImmutableArray<IObserver<LogEntry>> observers) : ILogger<TModule>
{
	public void Log(Severity level, string message, Exception? exception)
	{
		foreach (var observer in observers)
			observer.OnNext(new LogEntry(level, message, exception));
	}

	public bool IsEnabled(Severity level) =>
		true;
}
