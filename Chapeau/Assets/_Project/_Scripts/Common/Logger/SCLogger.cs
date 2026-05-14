using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine;

namespace Seacore.Logger
{
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
        /// Logs a message to the Unity console, prefixing it with the calling class name in a color-coded format.
        /// </summary>
        /// <remarks>The log entry is prefixed with the name of the calling class, displayed in a color
        /// derived from the class name. This can help distinguish log messages from different classes when reviewing
        /// console output.</remarks>
        /// <param name="message">The message object to log. The object's ToString() method will be called to obtain the string
        /// representation.</param>
        /// <param name="path">The full file path of the source code file that invoked the method. This parameter is automatically supplied
        /// by the compiler and is typically not set explicitly.</param>
        public static void Log(object message, [CallerFilePath] string path = "")
        {
            var className = Path.GetFileNameWithoutExtension(path);
            var colorCode = GetHexColor(className);
            Debug.Log($"<color=#{colorCode}><b>[{className}]</b></color> {message}");
        }
    }
}
