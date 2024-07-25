namespace PureWay.Ecs.Modules;

public partial class EcsModule
{
	// @formatter:keep_existing_arrangement true
	[Conditional("DI")]
	[UsedImplicitly]
	private static void Setup() =>
		DI.Setup()
		  .Hint(Hint.Resolve, "Off")
		  .Bind().To(context =>
		   {
			   context.Inject(out World world);
			   return world.Filter<TT>();
		   })
		  .Root<Filter<TT>>("Filter")
		  .Root<World>("World");
}
