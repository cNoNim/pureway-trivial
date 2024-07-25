using PureWay.Core.Diagnostics;

namespace PureWay.Sample;

public readonly struct OnLog(Action<LogEntry> action)
{
	public void Apply(LogEntry entry) =>
		action(entry);
}
