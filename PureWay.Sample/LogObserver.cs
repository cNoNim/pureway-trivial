using Leopotam.EcsLite;
using PureWay.Core.Diagnostics;

namespace PureWay.Sample;

public sealed class LogObserver(EcsWorld world) : IObserver<LogEntry>
{
	private readonly EcsFilter      _filter    = world.Filter<OnLog>().End();
	private readonly EcsPool<OnLog> _onLogPool = world.GetPool<OnLog>();
	public void OnCompleted() {}
	public void OnError(Exception error) {}

	public void OnNext(LogEntry entry)
	{
		foreach (var entity in _filter)
			_onLogPool.Get(entity).Apply(entry);
	}
}
