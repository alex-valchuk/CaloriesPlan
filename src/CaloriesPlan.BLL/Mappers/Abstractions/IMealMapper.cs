using System.Collections.Generic;

using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Mappers.Abstractions
{
    public interface IMealMapper
    {
        OutMealDto ConvertToDto(IMeal dbMeal);
        IList<OutMealDto> ConvertToDtoList(IList<IMeal> dbMeals);
    }
}
