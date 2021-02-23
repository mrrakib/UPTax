using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using UPTax.Model.ViewModels;
using UPTax.Data.Repository.Autofac;
using UPTax.Model.Models.Account;
using UPTax.Data.Repository;
using UPTax.Data.Infrastructure;

namespace UPTax.Service.Services.Autofac
{
    public interface IRoleService
    {
        IEnumerable<ApplicationRole> GetAll();
        IEnumerable<ApplicationRole> GetByRoleName(string roleName);
        ApplicationRole GetDetails(string roleId);
        bool Add(ApplicationRole role);
        bool CheckIfExist(string roleName);
        bool Update(ApplicationRole role);
        bool Save();
        bool Delete(string roleId);
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

        public bool Delete(string roleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationRole> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationRole> GetByRoleName(string roleName)
        {
            throw new NotImplementedException();
        }

        public ApplicationRole GetDetails(string roleId)
        {
            throw new NotImplementedException();
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
                return false;
            }
        }

        public bool Update(ApplicationRole role)
        {
            throw new NotImplementedException();
        }
    }
}
