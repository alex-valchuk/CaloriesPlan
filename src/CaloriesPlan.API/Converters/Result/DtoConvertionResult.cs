using System.Collections.Generic;
using System.Linq;

namespace CaloriesPlan.API.Converters.Result
{
    public class DtoConvertionResult
    {
        private IList<string> invalidProperties = new List<string>();

        public IEnumerable<string> InvalidProperties
        {
            get { return this.invalidProperties; }
        }

        public bool IsValid
        {
            get { return this.InvalidProperties.Count() == 0; }
        }

        public void AddInvalidProperty(string propertyName)
        {
            this.invalidProperties.Add(propertyName);
        }
    }
}