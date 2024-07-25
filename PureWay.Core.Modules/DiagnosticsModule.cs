using System.Collections.Immutable;
using PureWay.Core.Diagnostics;

namespace PureWay.Core.Modules;

public partial class DiagnosticsModule
{
	// @formatter:keep_existing_arrangement true
	[Conditional("DI")]
	[UsedImplicitly]
	private static void Setup() =>
		DI.Setup()
		  .Hint(Hint.Resolve, "Off")
		  .Bind().To<Logger<TT>>()
		  .RootArg<ImmutableArray<IObserver<LogEntry>>>("observers")
		  .Root<ILogger<TT>>("MakeLogger");
}
