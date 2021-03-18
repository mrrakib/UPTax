using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services
{
    public interface IMenuPermissionService
    {
        bool Add(MenuPermission model);
        bool Update(MenuPermission model);
        bool Delete(int id);
        IEnumerable<MenuPermission> GetAll();
        MenuPermission GetDetails(int id);
        bool Save();
        List<MenuPermission> GetAllPermittedMenues(string roleId, int categoryId);
        bool DeleteAllPermittedMenues(string roleId, int categoryId);
    }
    public class MenuPermissionService : IMenuPermissionService
    {
        private readonly IMenuPermissionRepository _menuPermissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuPermissionService(IMenuPermissionRepository menuPermissionRepository, IUnitOfWork unitOfWork)
        {
            _menuPermissionRepository = menuPermissionRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(MenuPermission model)
        {
            _menuPermissionRepository.Add(model);
            return true;
        }

        public bool Delete(int id)
        {
            MenuPermission model = _menuPermissionRepository.GetById(id);
            model.IsDeleted = true;

            _menuPermissionRepository.Update(model);
            return Save();
        }
        public bool Update(MenuPermission model)
        {
            _menuPermissionRepository.Update(model);
            return Save();
        }
        public IEnumerable<MenuPermission> GetAll()
        {
            return _menuPermissionRepository.GetMany(a => a.IsDeleted == false).ToList();
        }


        public MenuPermission GetDetails(int id)
        {
            return _menuPermissionRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public bool Save()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return false;
            }
        }

        public List<MenuPermission> GetAllPermittedMenues(string roleId, int categoryId)
        {
            return _menuPermissionRepository.GetAllPermittedMenues(roleId, categoryId);
        }

        public bool DeleteAllPermittedMenues(string roleId, int categoryId)
        {
            return _menuPermissionRepository.DeleteAllPermittedMenues(roleId, categoryId);
        }
    }
}
