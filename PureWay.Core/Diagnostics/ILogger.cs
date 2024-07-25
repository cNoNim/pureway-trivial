namespace PureWay.Core.Diagnostics;

public interface ILogger<out TModule>
{
	public void Log(Severity level, string message, Exception? exception);
	public bool IsEnabled(Severity level);
}
