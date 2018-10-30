using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ULinker.Common.Utility
{
    public static class Log4Helper
    {
        public static void Fatal(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Fatal(message);
            else
                log.Fatal(message, exception);
        }

        public static void Error(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Error(message);
            else
                log.Error(message, exception);
        }

        public static void Warn(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Warn(message);
            else
                log.Warn(message, exception);
        }

        public static void Info(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Info(message);
            else
                log.Info(message, exception);
        }

        public static void Debug(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Debug(message);
            else
                log.Debug(message, exception);
        }
    }
}
