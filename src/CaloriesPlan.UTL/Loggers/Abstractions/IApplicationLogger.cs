using System;

namespace CaloriesPlan.UTL.Loggers.Abstractions
{
    public interface IApplicationLogger
    {
        void Error(Exception ex);
    }
}
