using System.Collections.Generic;

using AutoMapper;

using CaloriesPlan.BLL.Mappers.Abstractions;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Mappers.AutoMappers
{
    public class MealAutoMapper : IMealMapper
    {
        public OutMealDto ConvertToDto(IMeal dbMeal)
        {
            return Mapper.Map<OutMealDto>(dbMeal);
        }

        public IList<OutMealDto> ConvertToDtoList(IList<IMeal> dbMeals)
        {
            return Mapper.Map<IList<OutMealDto>>(dbMeals);
        }
    }
}
