using Seacore.Logging;

namespace Seacore
{
    public static class Log
    {
        public static Logger gameManager = new Logger(new FileLogHandler("GameManager"));
        public static Logger diceManager = new Logger(new FileLogHandler("DiceManager"));
        
        public static void Debug(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#endif
        }

        public static void Warning(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(msg);
#endif
        }

        public static void Error(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(msg);
#endif
        }
    }
}

