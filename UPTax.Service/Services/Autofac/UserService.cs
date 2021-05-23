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
    }
}
