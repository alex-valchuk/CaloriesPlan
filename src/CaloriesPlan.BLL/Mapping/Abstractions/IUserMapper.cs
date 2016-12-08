using System.Collections.Generic;

using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Mapping.Abstractions
{
    public interface IUserMapper
    {
        OutUserDto ConvertToUserDto(IUser userModel);
        IList<OutUserDto> ConvertToUserDtoList(IList<IUser> userModels);

        OutUserProfileDto ConvertToUserProfileDto(IUser userModel, IList<IRole> roleModels);
        IList<OutUserRoleDto> ConvertToUserRoleDtoList(IList<IRole> roleModels);

        IList<OutShortUserInfoDto> ConvertToOutShortUserInfoDtoList(IList<IUser> models);
        OutShortUserInfoDto ConvertToOutShortUserInfoDto(IUser model);
    }
}
