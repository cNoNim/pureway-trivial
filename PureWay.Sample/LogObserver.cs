using PureWay.Core.Diagnostics;

namespace PureWay.Sample;

public sealed class LogObserver(World world) : IObserver<LogEntry>
{
	private readonly Filter<OnLog>      _filter    = world.Filter<OnLog>();
	public void OnCompleted() {}
	public void OnError(Exception error) {}

	public void OnNext(LogEntry entry)
	{
		foreach (var onLog in _filter)
			onLog.Apply(entry);
	}
}
