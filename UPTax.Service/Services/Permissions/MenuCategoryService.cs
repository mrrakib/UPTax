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
    public interface IMenuCategoryService
    {
        bool Add(MenuCategory model);
        bool Update(MenuCategory model);
        bool Delete(int id);
        IPagedList GetPagedList(string keyName, int pageNo, int pageSize);
        IEnumerable<MenuCategory> GetAll();
        MenuCategory GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
        List<IdNameDropdown> GetMenuCategoryDDL();
    }
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuCategoryService(IMenuCategoryRepository menuCategoryRepository, IUnitOfWork unitOfWork)
        {
            _menuCategoryRepository = menuCategoryRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(MenuCategory model)
        {
            _menuCategoryRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            MenuCategory model = _menuCategoryRepository.GetById(id);
            model.IsDeleted = true;

            _menuCategoryRepository.Update(model);
            return Save();
        }
        public bool Update(MenuCategory model)
        {
            _menuCategoryRepository.Update(model);
            return Save();
        }
        public IEnumerable<MenuCategory> GetAll()
        {
            return _menuCategoryRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public List<IdNameDropdown> GetMenuCategoryDDL()
        {
            return _menuCategoryRepository.GetMany(a => a.IsDeleted == false).Select(e => new IdNameDropdown
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
        }

        public MenuCategory GetDetails(int id)
        {
            return _menuCategoryRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@"  WHERE Name LIKE N'%{0}%' AND IsDeleted = 0", keyName.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE IsDeleted = 0");
                }
                string query = string.Format(@"SELECT * FROM MenuCategories {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(Id) FROM MenuCategories WHERE Name LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _menuCategoryRepository.SQLQuery<int>(countQuery);
                List<MenuCategory> villages = _menuCategoryRepository.SQLQueryList<MenuCategory>(query).ToList();
                return new StaticPagedList<MenuCategory>(villages, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<MenuCategory>(new List<MenuCategory> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _menuCategoryRepository.GetCount(a => a.IsDeleted == false && a.Name == keyName.Trim());
            else
                count = _menuCategoryRepository.GetCount(a => a.IsDeleted == false && a.Name == keyName.Trim() && a.Id != id);
            return count > 0 ? true : false;
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



    }
}
