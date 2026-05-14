using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine;
using UnityEditor.SearchService;
using System;

namespace Seacore.Logger
{
    /// <summary>
    /// Provides static methods for writing debug, warning, and error log entries with contextual information, such as
    /// the calling source file, to assist with application debugging and diagnostics.
    /// </summary>
    /// <remarks>The SCLogger class is designed for use during development to enhance log output with source
    /// context, making it easier to trace log messages to their origin in the codebase. All methods are thread-safe and
    /// can be called from any context. The class is intended for use with Unity's logging system and is not suitable
    /// for production logging scenarios where persistent or structured logging is required.</remarks>
    public static class SCLogger
    {
        /// <summary>
        /// Generates a deterministic hexadecimal RGB color code based on the specified name.
        /// </summary>
        /// <remarks>The same input name will always produce the same color code. The output does not
        /// include a leading '#' character.</remarks>
        /// <param name="name">The input string used to generate the color. Cannot be null.</param>
        /// <returns>A six-character hexadecimal string representing the RGB color derived from the input name.</returns>
        private static string GetHexColor(string name)
        {
            var hue = (uint)name.GetHashCode() / (float)uint.MaxValue;
            var color = Color.HSVToRGB(hue, 0.6f, 1f);
            return ColorUtility.ToHtmlStringRGB(color);
        }

        /// <summary>
        /// Writes a debug log entry with the specified message and source file path.
        /// </summary>
        /// <remarks>This method is intended for use during development to assist with debugging. The log
        /// entry includes the provided message and the file path from which the method was called.</remarks>
        /// <param name="message">The message object to log. The object's string representation is written to the debug output.</param>
        /// <param name="path">The full path of the source file that invoked the log method. This value is automatically provided by the
        /// compiler and should not be set manually.</param>
        public static void Log(object message, [CallerFilePath] string path = "") => LogType(Debug.Log, message, path);
        /// <summary>
        /// Logs a warning message to the debug output.
        /// </summary>
        /// <param name="message">The message object to log. The object's string representation is written to the output.</param>
        /// <param name="path">The full path of the source file that invoked the method. This parameter is automatically provided by the
        /// compiler and should not be set explicitly.</param>
        public static void LogWarning(object message, [CallerFilePath] string path = "") => LogType(Debug.LogWarning, message, path);
        /// <summary>
        /// Logs an error message to the debug output.
        /// </summary>
        /// <param name="message">The message object to log. The object's string representation is written to the error log.</param>
        /// <param name="path">The full path of the source file that invoked the method. This parameter is automatically provided by the
        /// compiler and should not be set explicitly.</param>
        public static void LogError(object message, [CallerFilePath] string path = "") => LogType(Debug.LogError, message, path);
        /// <summary>
        /// Logs a message to the Unity Console with an optional context object and source file path information.
        /// </summary>
        /// <param name="message">The message object to log. If the object is not a string, its ToString method will be called to convert it
        /// to a string representation.</param>
        /// <param name="context">The UnityEngine.Object context to associate with the log message. This can be used to highlight the object
        /// in the editor when the log entry is selected. Can be null.</param>
        /// <param name="path">The full file path of the source code file that invoked the log method. This is typically supplied
        /// automatically by the compiler and is used for debugging purposes. Optional.</param>
        public static void Log(object message, UnityEngine.Object context, [CallerFilePath] string path = "") => LogType(Debug.Log, message, context, path);
        /// <summary>
        /// Logs a warning message to the Unity Console with an optional context object and source file path.
        /// </summary>
        /// <remarks>Use this method to report recoverable issues or unexpected situations that do not
        /// prevent the application from continuing. The message will appear in the Unity Console as a warning. If a
        /// context object is provided, clicking the log entry in the Console will highlight the associated object in
        /// the Editor.</remarks>
        /// <param name="message">The message object to log. This is converted to a string representation in the output.</param>
        /// <param name="context">An optional UnityEngine.Object that provides context for the log message. Can be used to highlight a
        /// specific object in the Unity Editor. May be null.</param>
        /// <param name="path">The full file path of the source code file that invoked the log method. This is typically supplied
        /// automatically by the compiler and should not be set manually.</param>
        public static void LogWarning(object message, UnityEngine.Object context, [CallerFilePath] string path = "") => LogType(Debug.LogWarning, message, context, path);
        /// <summary>
        /// Logs an error message to the Unity Console, optionally associating it with a specific context object and
        /// source file path.
        /// </summary>
        /// <remarks>Use this method to report errors that should be visible in the Unity Console.
        /// Associating a context object helps identify the source of the error in the scene or project. The optional
        /// path parameter can assist with debugging by indicating the source file location.</remarks>
        /// <param name="message">The error message to log. Can be any object; its string representation will be written to the console.</param>
        /// <param name="context">The Unity object to associate with the error message. This can be used to highlight the object in the editor
        /// when the message is selected. Can be null.</param>
        /// <param name="path">The full file path of the source code file from which the log call originates. This is typically supplied
        /// automatically by the compiler and should not be set manually.</param>
        public static void LogError(object message, UnityEngine.Object context, [CallerFilePath] string path = "") => LogType(Debug.LogError, message, context, path);



        private static void LogType(Action<object> loggingFunc, object message, [CallerFilePath] string path = "")
        {
            var className = Path.GetFileNameWithoutExtension(path);
            var colorCode = GetHexColor(className);
            loggingFunc($"<color=#{colorCode}><b>[{className}]</b></color> {message}");
        }

        private static void LogType(Action<object, UnityEngine.Object> loggingFunc, object message, UnityEngine.Object context, [CallerFilePath] string path = "")
        {
            var className = Path.GetFileNameWithoutExtension(path);
            var colorCode = GetHexColor(className);
            loggingFunc($"<color=#{colorCode}><b>[{className}]</b></color> {message}", context);
        }
    }
}
