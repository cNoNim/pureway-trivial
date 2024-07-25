using Leopotam.EcsLite;

namespace PureWay.Ecs.Modules;

public partial class EcsModule
{
	// @formatter:keep_existing_arrangement true
	[Conditional("DI")]
	[UsedImplicitly]
	private static void Setup() =>
		DI.Setup()
		  .Hint(Hint.Resolve, "Off")
		  .Root<EcsWorld>("World");
}
