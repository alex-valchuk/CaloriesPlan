using System;

using CaloriesPlan.UTL.Loggers.Abstractions;

using NLog;

namespace CaloriesPlan.UTL.Loggers
{
    public class NLogger : IApplicationLogger
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Error(Exception ex)
        {
            logger.Error(ex);
        }
    }
}
