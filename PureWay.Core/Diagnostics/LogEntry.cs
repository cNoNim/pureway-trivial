namespace PureWay.Core.Diagnostics;

public readonly record struct LogEntry(Severity Level, string Message, Exception? Exception);
