using UnityEngine;

namespace Seacore
{
    namespace Logging
    {
        /// <summary>
        /// Wrapper class with ifdef's
        /// </summary>
        public class Logger : UnityEngine.Logger
        {
            public Logger(ILogHandler logHandler) : base(logHandler)
            {
            }

            public new void Log(LogType logType, object message)
            {
    #if UNITY_EDITOR
                base.Log(logType, message);
    #endif
            }
            public new void Log(LogType logType, object message, Object context)
            {
    #if UNITY_EDITOR
                base.Log(logType, message, context);
    #endif
            }
            public new void Log(LogType logType, string tag, object message)
            {
    #if UNITY_EDITOR
                base.Log(logType, tag, message);
    #endif
            }
            public new void Log(LogType logType, string tag, object message, Object context)
            {
    #if UNITY_EDITOR
                base.Log(logType, tag, message, context);
    #endif
            }
            public new void Log(object message)
            {
    #if UNITY_EDITOR
                base.Log(message);
    #endif
            }
            public new void Log(string tag, object message)
            {
    #if UNITY_EDITOR
                base.Log(tag, message);
    #endif
            }
            public new void Log(string tag, object message, Object context)
            {
    #if UNITY_EDITOR
                base.Log(tag, message, context);
    #endif
            }
            public new void LogWarning(string tag, object message)
            {
    #if UNITY_EDITOR
                base.LogWarning(tag, message);
    #endif
            }
            public new void LogWarning(string tag, object message, Object context)
            {
    #if UNITY_EDITOR
                base.LogWarning(tag, message, context);
    #endif
            }
            public new void LogError(string tag, object message)
            {
    #if UNITY_EDITOR
                base.LogError(tag, message);
    #endif
            }
            public new void LogError(string tag, object message, Object context)
            {
    #if UNITY_EDITOR
                base.LogError(tag, message, context);
    #endif
            }
            public new void LogException(System.Exception exception)
            {
    #if UNITY_EDITOR
                base.LogException(exception);
    #endif
            }
            public new void LogException(System.Exception exception, Object context)
            {
    #if UNITY_EDITOR
                base.LogException(exception, context);
    #endif
            }
            public new void LogFormat(LogType logType, string format, params object[] args)
            {
    #if UNITY_EDITOR
                base.LogFormat(logType, format, args);
    #endif
            }
            public new void LogFormat(LogType logType, Object context, string format, params object[] args)
            {
    #if UNITY_EDITOR
                base.LogFormat(logType, context, format, args);
    #endif
            }
        }
    }
}
