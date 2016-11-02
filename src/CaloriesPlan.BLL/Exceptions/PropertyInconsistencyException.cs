using System;

namespace CaloriesPlan.BLL.Exceptions
{
    public class PropertyInconsistencyException : Exception
    {
        public string PropertyName { get; private set; }

        public PropertyInconsistencyException(string propertyName, string message)
            : base(message)
        {
            this.PropertyName = propertyName;
        }
    }
}
