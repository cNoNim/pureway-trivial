using PureWay.Core.Diagnostics;

namespace PureWay.Sample;

public abstract class GameBase(ILogger<GameBase> logger)
{
	protected readonly ILogger<GameBase> Logger = logger;
}
