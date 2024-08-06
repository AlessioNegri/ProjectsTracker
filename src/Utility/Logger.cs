using System.IO;

namespace ProjectsTracker.src.Utility
{
    /// <summary> Type of message </summary>
    enum MessageType : int
    {
        INFO    = 0,
        WARNING = 1,
        ERROR   = 2
    }

    /// <summary> Class to manage log messages </summary>
    class Logger
    {
        #region MEMBERS

        /// <summary> Singleton instance </summary>
        private static Logger? instance = null;

        /// <summary> Thread lock </summary>
        private static readonly object padlock = new object();

        /// <summary> Log file path </summary>
        private string file = string.Empty;

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Retrieves the class instance </summary>
        public static Logger Instance
        {
            get { lock (padlock) { if (instance is null) instance = new Logger(); return instance; } }
        }

        /// <summary> Initializes the logger </summary>
        public void Init()
        {
            string folder = Directory.GetCurrentDirectory() + "/log";

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            file = folder + $"/ProjectsTracker_" + DateTime.Now.ToString(format: "yyyy-MM-dd_HH.mm.ss") + ".txt";

            using (StreamWriter sw = File.CreateText(file)) {}

            Info("Logger Started...");
        }

        /// <summary> Closes the logger </summary>
        public void Close()
        {
            Info("Logger Stopped.");
        }

        /// <summary> Writes an INFO message in the log </summary>
        public void Info(string message)
        {
            using (StreamWriter sw = File.AppendText(file)) sw.WriteLine(Format(message, MessageType.INFO));
        }

        /// <summary> Writes an WARNING message in the log </summary>
        public void Warning(string message)
        {
            using (StreamWriter sw = File.AppendText(file)) sw.WriteLine(Format(message, MessageType.WARNING));
        }

        /// <summary> Writes an ERROR message in the log </summary>
        public void Error(string message)
        {
            using (StreamWriter sw = File.AppendText(file)) sw.WriteLine(Format(message, MessageType.ERROR));
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Constructor </summary>
        private Logger() { }

        /// <summary> Formats the message </summary>
        /// <param name="message"> Original message </param>
        /// <param name="type"> Type of message </param>
        /// <returns> Formatted message </returns>
        private string Format(string message, MessageType type)
        {
            // Prefix

            string formatted_message = string.Empty;

            switch (type)
            {
                case MessageType.INFO:      formatted_message += "[ INFO    ]"; break;
                case MessageType.WARNING:   formatted_message += "[ WARNING ]"; break;
                case MessageType.ERROR:     formatted_message += "[ ERROR   ]"; break;
            }

            formatted_message += "\t";

            // Date-Time

            formatted_message += $"<{DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss")}>";

            formatted_message += "\t";

            // Message

            formatted_message += message;

            // Formatted message

            return formatted_message;
        }

        #endregion
    }
}