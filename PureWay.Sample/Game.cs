using Leopotam.EcsLite;
using PureWay.Core.Diagnostics;

namespace PureWay.Sample;

public sealed class Game : GameBase
{
	public Game(EcsWorld world, ILogger<Game> logger)
		: base(logger)
	{
		var entity = world.NewEntity();
		world.GetPool<KeepAlive>().Add(entity);
	}

	public void Run() =>
		Logger.Log(Severity.Information, $"Starting");
}
