using System.Collections.Immutable;
using PureWay.Core.Diagnostics;
using PureWay.Core.Modules;
using PureWay.Ecs.Modules;

namespace PureWay.Sample;

public partial class GameModule
{
	// @formatter:keep_existing_arrangement true
	[Conditional("DI")]
	[UsedImplicitly]
	private static void Setup() =>
		DI.Setup()
		  .Hint(Hint.Resolve,  "Off")
		  .Root<Game>(nameof(Game))
		  .Bind().As(Singleton).To<DiagnosticsModule>()
		  .Bind<ILogger<TT>>().To(
			   context =>
			   {
				   context.Inject(out DiagnosticsModule module);
				   context.Inject(out ImmutableArray<IObserver<LogEntry>> logObservers);
				   return module.MakeLogger<TT>(logObservers);
			   })
		  .Bind().As(Singleton).To<EcsModule>()
		  .Bind<Filter<TT>>().To(
			   context =>
			   {
				   context.Inject(out EcsModule module);
				   return module.Filter<TT>();
			   })
		  .Bind().To(
			   context =>
			   {
				   context.Inject(out EcsModule module);
				   return module.World;
			   })
		  .DefaultLifetime(PerResolve)
		  .Bind().To<LogObserver>();
}
