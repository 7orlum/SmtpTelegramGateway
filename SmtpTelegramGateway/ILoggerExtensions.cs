﻿using SmtpServer;
using SmtpServer.Net;
using SmtpServer.Tracing;

namespace SmtpTelegramGateway;

internal static class ILoggerExtensions
{
    private static readonly Action<ILogger, string, Exception?> _error =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            0,
            "{Message}");

    private static readonly Action<ILogger, string, Exception?> _debugTelegramSendingMessage =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            0,
            "Telegram sending message to {Chat}");

    private static readonly Action<ILogger, object, Exception?> _debugSessionCreated =
        LoggerMessage.Define<object>(
            LogLevel.Debug,
            1,
            "SMTP session {Session} created");

    private static readonly Action<ILogger, object, Exception?> _debugSessionCompleted =
        LoggerMessage.Define<object>(
            LogLevel.Debug,
            2,
            "SMTP session {Session} completed");

    private static readonly Action<ILogger, object, Exception?> _debugSessionCancelled =
        LoggerMessage.Define<object>(
            LogLevel.Debug,
            3,
            "SMTP session {Session} cancelled");

    private static readonly Action<ILogger, object, Exception?> _debugSessionFaulted =
        LoggerMessage.Define<object>(
            LogLevel.Debug,
            4,
            "SMTP session {Session} faulted");

    private static readonly Action<ILogger, object, string, Exception?> _debugCommandExecuting =
        LoggerMessage.Define<object, string>(
            LogLevel.Debug,
            5,
            "SMTP session {Session} command {Command}");

    public static void LogError(this ILogger logger, Exception e)
    {
        _error(logger, e.Message, e);
    }

    public static void LogSmtpSessionCreated(this ILogger logger, SessionEventArgs e)
    {
        var session = e.Context.Properties[EndpointListener.RemoteEndPointKey];
        _debugSessionCreated(logger, session, default);
    }

    public static void LogSmtpSessionCompleted(this ILogger logger, SessionEventArgs e)
    {
        var session = e.Context.Properties[EndpointListener.RemoteEndPointKey];
        _debugSessionCompleted(logger, session, default);
    }

    public static void LogSmtpSessionCancelled(this ILogger logger, SessionEventArgs e)
    {
        var session = e.Context.Properties[EndpointListener.RemoteEndPointKey];
        _debugSessionCancelled(logger, session, default);
    }

    public static void LogSmtpSessionFaulted(this ILogger logger, SessionFaultedEventArgs e)
    {
        var session = e.Context.Properties[EndpointListener.RemoteEndPointKey];
        _debugSessionFaulted(logger, session, default);
    }

    public static void LogSmtpCommandExecuting(this ILogger logger, SmtpCommandEventArgs e)
    {
        var session = e.Context.Properties[EndpointListener.RemoteEndPointKey];

        using var writer = new StringWriter();
        new TracingSmtpCommandVisitor(writer).Visit(e.Command);
        var command = writer.ToString();

        _debugCommandExecuting(logger, session, command, default);
    }

    public static void LogTelegramSendingMessage(this ILogger logger, string chat)
    {
        _debugTelegramSendingMessage(logger, chat, default);
    }

}
