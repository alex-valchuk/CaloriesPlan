using System.Collections.Generic;

using CaloriesPlan.BLL.Mapping.Abstractions;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Mapping
{
    public class MealMapper : IMealMapper
    {
        public OutMealDto ConvertToDto(IMeal mealModel)
        {
            var dto = new OutMealDto();
            dto.ID = mealModel.ID;
            dto.Calories = mealModel.Calories;
            dto.EatingDate = mealModel.EatingDate;
            dto.Text = mealModel.Text;

            return dto;
        }

        public IList<OutMealDto> ConvertToDtoList(IList<IMeal> models)
        {
            var dtoList = new List<OutMealDto>();

            foreach (var model in models)
            {
                var dto = this.ConvertToDto(model);
                dtoList.Add(dto);
            }

            return dtoList;
        }
    }
}
