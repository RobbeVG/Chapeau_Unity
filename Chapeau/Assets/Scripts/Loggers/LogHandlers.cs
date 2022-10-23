using System;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

namespace Seacore
{
    namespace Logging 
    {
        public class BaseLogHandler : ILogHandler
        {
            protected string _name;
            private static ILogHandler _defaultDebugLogHandler = Debug.unityLogger.logHandler; //Could be changed during runtime! 

            public BaseLogHandler(string name) 
            {
                _name = name; 
            }

            public virtual void LogException(Exception exception, UnityEngine.Object context)
            {
                _defaultDebugLogHandler.LogException(exception, context);
            }

            public virtual void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                string formatPrefix = $"<b>[{_name}]</b> ";
                if (logType == LogType.Error)
                {
                    formatPrefix = $"<color=red><b>[{_name}]</b></color> ";
                }
                else if (logType == LogType.Warning)
                {
                    formatPrefix = $"<color=orange><b>[{_name}]</b></color> ";
                }
                else if (logType == LogType.Assert)
                {
                    formatPrefix = $"<color=darkblue><b>[{_name}]</b></color> ";
                }

                _defaultDebugLogHandler.LogFormat(logType, context, formatPrefix + format, args);
            }
        }

        public class FileLogHandler : BaseLogHandler
        {
            private FileStream _fileStream = null;
            private StreamWriter _streamWriter = null;

            bool _consoleLogging = true;

            public FileLogHandler(string name, bool consoleLogging = true)
                : base(name)
            {
                _consoleLogging = consoleLogging;
                _fileStream = new FileStream(Application.persistentDataPath + "/" + _name + "_log.txt", FileMode.Create, FileAccess.ReadWrite);
                _streamWriter = new StreamWriter(_fileStream);
            }

            public override void LogException(Exception exception, UnityEngine.Object context)
            {
                _streamWriter.WriteLine(exception.GetType().ToString() + '\n' + exception.Message);
                base.LogException(exception, context);
            }

            public override void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {

                string formatFile = "[" + string.Format("{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) + "]" + format;
                
                _streamWriter.WriteLine(string.Format(formatFile, args));
                _streamWriter.Flush();

                if (_consoleLogging)
                    base.LogFormat(logType, context, format, args);

            }
        }
    }
}
