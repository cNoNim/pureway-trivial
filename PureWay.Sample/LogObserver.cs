using PureWay.Core.Diagnostics;

namespace PureWay.Sample;

public sealed class LogObserver(Filter<OnLog> filter) : IObserver<LogEntry>
{
	public void OnCompleted() {}
	public void OnError(Exception error) {}

	public void OnNext(LogEntry entry)
	{
		foreach (var onLog in filter)
			onLog.Apply(entry);
	}
}
