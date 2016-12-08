using System.Collections.Generic;

using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Mapping.Abstractions
{
    public interface IMealMapper
    {
        OutMealDto ConvertToDto(IMeal mealModel);
        IList<OutMealDto> ConvertToDtoList(IList<IMeal> mealModels);
    }
}
