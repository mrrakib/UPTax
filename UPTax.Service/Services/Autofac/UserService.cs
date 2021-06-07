using PagedList;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Repository.Autofac;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services.Autofac
{
    public interface IUserService
    {
        IPagedList GetUserPaged(int pageNo, int pageSize);
        List<IdNameDropdown> GetAllForDropdown();
        VMMyProfile GetMyProfileData(int unionId);
        VMMyProfile GetMyProfileDataAdmin(string userId, int unionId);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<IdNameDropdown> GetAllForDropdown()
        {
            var users = new List<IdNameDropdown>()
            {
                new IdNameDropdown(){ IdStr ="", Name = "Select..." }
            };
            return users;
        }

        public IPagedList GetUserPaged(int pageNo, int pageSize)
        {
            string query = @"SELECT U.UserName, 
                            U.FullName, R.Name as RoleName, U.IsActive AS Status, U.Id as UserId, R.Id AS RoleId
                            FROM Users U
                            JOIN UserRoles UR ON U.Id = UR.UserId
                            JOIN Roles R ON UR.RoleId = R.Id";
            List<VMUserInfo> userList = _userRepository.SQLQueryList<VMUserInfo>(query).ToList();
            int rowCount = userList.Count;
            var result = userList.OrderBy(u => u.UserName).Skip((pageNo - 1) * pageSize).Take(pageSize);
            return new StaticPagedList<VMUserInfo>(result, pageNo, pageSize, rowCount);
        }

        public VMMyProfile GetMyProfileData(int unionId)
        {
            string query = string.Format(@"SELECT u.FullName, u.PhoneNumber, u.Email, up.ImagePath, up.Name UnionName, up.Email UpMail
                            , up.PhoneNo UpPhone FROM Users u
                            LEFT JOIN UnionParishad up ON U.UnionId = up.Id
                            WHERE up.Id = {0}", unionId);
            VMMyProfile userList = _userRepository.SQLQuery<VMMyProfile>(query);
            return userList;
        }
        public VMMyProfile GetMyProfileDataAdmin(string userId, int unionId)
        {
            string queryUser = string.Format(@"SELECT u.Id, u.FullName, u.PhoneNumber, u.Email FROM Users u WHERE u.Id = '{0}'", userId);
            VMMyProfile user = _userRepository.SQLQuery<VMMyProfile>(queryUser);
            string queryUp = string.Format(@"SELECT up.ImagePath, up.Name UnionName, up.Email UpMail
, up.PhoneNo UpPhone FROM UnionParishad up WHERE up.Id = {0}", unionId);
            VMMyProfile up = _userRepository.SQLQuery<VMMyProfile>(queryUp);

            VMMyProfile profileData = new VMMyProfile();
            if (user != null && up != null)
            {
                profileData.Email = user.Email;
                profileData.FullName = user.FullName;
                profileData.PhoneNumber = user.PhoneNumber;
                profileData.UnionName = up.UnionName;
                profileData.ImagePath = up.ImagePath;
                profileData.UpMail = up.UpMail;
                profileData.UpPhone = up.UpPhone;
            }

            return profileData;
        }
    }
}
