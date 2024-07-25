using System.Runtime.CompilerServices;

namespace PureWay.Core.Diagnostics;

[InterpolatedStringHandler]
public ref struct LogInterpolatedStringHandler<TModule>
{
	private DefaultInterpolatedStringHandler _handler;

	public LogInterpolatedStringHandler(
		int literalLength,
		int formattedCount,
		ILogger<TModule> logger,
		Severity severity,
		out bool isEnabled)
	{
		IsEnabled = isEnabled = logger.IsEnabled(severity);
		_handler  = isEnabled ? new DefaultInterpolatedStringHandler(literalLength, formattedCount) : default;
	}

	public bool IsEnabled { get; }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AppendFormatted<T>(T value, int alignment = 0, string? format = null)
	{
		_handler.AppendFormatted(value, alignment, format);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AppendLiteral(string? str)
	{
		if (!string.IsNullOrEmpty(str))
			_handler.AppendLiteral(str);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal string GetAndReset()
	{
		var result = _handler.ToStringAndClear();
		_handler = default;
		return result;
	}
}
