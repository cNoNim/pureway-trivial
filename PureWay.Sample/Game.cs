using PureWay.Core.Diagnostics;

namespace PureWay.Sample;

public sealed class Game : GameBase
{
	public Game(World world, ILogger<Game> logger)
		: base(logger)
	{
		var entity = world.Create();
		world.Add<KeepAlive>(entity);
	}

	public void Run() =>
		Logger.Log(Severity.Information, $"Starting");
}
