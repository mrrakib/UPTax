using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models.Account;

namespace UPTax.Service.Services.Permissions
{
    public interface IRoleService
    {
        IEnumerable<ApplicationRole> GetAll();
        ApplicationRole GetByRoleName(string roleName);
        ApplicationRole GetDetails(string roleId);
        bool Add(ApplicationRole role);
        bool CheckIfExist(string roleName);
        bool Update(ApplicationRole role);
        bool Save();
        bool Delete(string roleId);
        IPagedList<ApplicationRole> GetPageList(Page page, string name);
        bool CheckIfExistForUpdate(string roleName, string roleId);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public bool Add(ApplicationRole role)
        {
            _roleRepository.Add(role);
            return Save();
        }

        public bool CheckIfExist(string roleName)
        {
            int countRole = _roleRepository.GetMany(r => r.Name == roleName).Count();
            if (countRole > 0)
                return true;
            else
                return false;
        }

        public bool CheckIfExistForUpdate(string roleName, string roleId)
        {
            int countRole = _roleRepository.GetMany(r => r.Name == roleName && !r.Id.Equals(roleId)).Count();
            if (countRole > 0)
                return true;
            else
                return false;
        }

        public bool Delete(string roleId)
        {
            ApplicationRole role = _roleRepository.Get(r => r.Id.Equals(roleId));
            _roleRepository.Delete(role);
            return Save();
        }

        public IEnumerable<ApplicationRole> GetAll()
        {
            return _roleRepository.GetAll();
        }

        public ApplicationRole GetByRoleName(string roleName)
        {
            return _roleRepository.Get(r => r.Name.Equals(roleName));
        }

        public ApplicationRole GetDetails(string roleId)
        {
            return _roleRepository.Get(r => r.Id.Equals(roleId));
        }

        public bool Save()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception e)
            {
                var errorMessage = e.Message;
                return false;
            }
        }

        public bool Update(ApplicationRole role)
        {
            _roleRepository.Update(role);
            return Save();
        }

        public IPagedList<ApplicationRole> GetPageList(Page page, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return _roleRepository.GetPage(page, x => true && x.Name.StartsWith(name), order => order.Name);
            }
            return _roleRepository.GetPage(page, x => true, order => order.Name);
        }
    }
}
