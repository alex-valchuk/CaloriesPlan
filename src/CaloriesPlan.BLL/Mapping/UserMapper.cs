using System.Collections.Generic;

using CaloriesPlan.BLL.Mapping.Abstractions;
using CaloriesPlan.DAL.DataModel.Abstractions;
using CaloriesPlan.DTO.Out;

namespace CaloriesPlan.BLL.Mapping
{
    public class UserMapper : IUserMapper
    {
        public OutUserDto ConvertToUserDto(IUser model)
        {
            if (model == null)
                return null;

            var dto = new OutUserDto();
            this.MapUserDto(model, dto);

            return dto;
        }

        private void MapUserDto<TOut>(IUser model, TOut dto) where TOut : OutUserDto
        {
            dto.DailyCaloriesLimit = model.DailyCaloriesLimit;
            dto.UserName = model.UserName;
        }

        public IList<OutUserDto> ConvertToUserDtoList(IList<IUser> models)
        {
            if (models == null)
                return null;

            var dtoList = new List<OutUserDto>();

            foreach (var model in models)
            {
                var dto = this.ConvertToUserDto(model);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public OutUserProfileDto ConvertToUserProfileDto(IUser userModel, IList<IRole> roleModels)
        {
            if (userModel == null)
                return null;

            var dto = new OutUserProfileDto();
            this.MapUserDto(userModel, dto);

            dto.UserRoles = this.ConvertToUserRoleDtoList(roleModels);

            return dto;
        }

        public OutUserRoleDto ConvertToUserRoleDto(IRole model)
        {
            if (model == null)
                return null;

            var dto = new OutUserRoleDto();
            dto.RoleName = model.Name;

            return dto;
        }

        public IList<OutUserRoleDto> ConvertToUserRoleDtoList(IList<IRole> models)
        {
            if (models == null)
                return null;

            var dtoList = new List<OutUserRoleDto>();

            foreach (var model in models)
            {
                var dto = this.ConvertToUserRoleDto(model);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public IList<OutShortUserInfoDto> ConvertToOutShortUserInfoDtoList(IList<IUser> models)
        {
            if (models == null)
                return null;

            var dtoList = new List<OutShortUserInfoDto>();

            foreach (var model in models)
            {
                var dto = this.ConvertToOutShortUserInfoDto(model);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public OutShortUserInfoDto ConvertToOutShortUserInfoDto(IUser model)
        {
            if (model == null)
                return null;

            var dto = new OutShortUserInfoDto();
            dto.UserName = model.UserName;

            return dto;
        }
    }
}
