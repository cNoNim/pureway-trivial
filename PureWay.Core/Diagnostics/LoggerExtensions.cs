using System.Runtime.CompilerServices;

namespace PureWay.Core.Diagnostics;

public static class LoggerExtensions
{
	public static void Log<TModule>(
		this ILogger<TModule> logger,
		Severity severity,
		[InterpolatedStringHandlerArgument("logger", "severity")] ref LogInterpolatedStringHandler<TModule> handler)
	{
		if (handler.IsEnabled)
			Log(
				logger,
				severity,
				null,
				handler.GetAndReset());
	}

	public static void Log<TModule>(
		this ILogger<TModule> logger,
		Severity severity,
		Exception exception,
		[InterpolatedStringHandlerArgument("logger", "severity")]
		ref LogInterpolatedStringHandler<TModule> handler)
	{
		if (handler.IsEnabled)
			Log(
				logger,
				severity,
				exception,
				handler.GetAndReset());
	}

	private static void Log<TModule>(
		ILogger<TModule> logger,
		Severity severity,
		Exception? exception,
		string entry) =>
		logger.Log(severity, entry, exception);
}
