using System.Threading.Tasks;

using CaloriesPlan.DTO.In;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Services.Abstractions
{
    public interface IMealService
    {
        Task<OutNutritionReportDto> GetUserNutritionReportAsync(string userName, InMealReportFilterDto filter);
        Task<OutMealDto> GetMealByIDAsync(int mealID);
        Task CreateMealAsync(string userName, InMealDto mealDto);
        Task UpdateMealAsync(int id, InMealDto mealDto);
        Task DeleteMealAsync(int id);
        Task<bool> IsOwnerOfMealAsync(string userName, int mealID);
    }
}
