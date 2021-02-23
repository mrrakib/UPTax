using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using UPTax.Model.ViewModels;
using UPTax.Data.Repository.Autofac;

namespace UPTax.Service.Services.Autofac
{
    public interface IUserService
    {
        IPagedList GetUserPaged(int pageNo, int pageSize);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
