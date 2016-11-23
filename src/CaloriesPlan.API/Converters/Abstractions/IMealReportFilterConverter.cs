using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CaloriesPlan.API.Converters.Result;
using CaloriesPlan.DTO.In;

namespace CaloriesPlan.API.Converters.Abstractions
{
    public interface IMealReportFilterConverter
    {
        DtoConvertionResult Validate(InMealReportFilterDto dto);
    }
}
