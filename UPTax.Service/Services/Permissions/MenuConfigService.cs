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
    public interface IMenuConfigService
    {
        bool Add(MenuConfig model);
        bool Update(MenuConfig model);
        bool Delete(int id);
        IPagedList GetPagedList(string keyName, int pageNo, int pageSize);
        IEnumerable<MenuConfig> GetAll();
        MenuConfig GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
    }
    public class MenuConfigService : IMenuConfigService
    {
        private readonly IMenuConfigRepository _menuConfigRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuConfigService(IMenuConfigRepository menuConfigRepository, IUnitOfWork unitOfWork)
        {
            _menuConfigRepository = menuConfigRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(MenuConfig model)
        {
            _menuConfigRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            MenuConfig model = _menuConfigRepository.GetById(id);
            model.IsDeleted = true;

            _menuConfigRepository.Update(model);
            return Save();
        }
        public bool Update(MenuConfig model)
        {
            _menuConfigRepository.Update(model);
            return Save();
        }
        public IEnumerable<MenuConfig> GetAll()
        {
            return _menuConfigRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public MenuConfig GetDetails(int id)
        {
            return _menuConfigRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@"  WHERE MenuName LIKE N'%{0}%' AND IsDeleted = 0", keyName.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE IsDeleted = 0");
                }
                string query = string.Format(@"SELECT * FROM MenuConfigs {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(Id) FROM MenuConfigs WHERE MenuName LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _menuConfigRepository.SQLQuery<int>(countQuery);
                List<MenuConfig> models = _menuConfigRepository.SQLQueryList<MenuConfig>(query).ToList();
                return new StaticPagedList<MenuConfig>(models, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<MenuConfig>(new List<MenuConfig> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _menuConfigRepository.GetCount(a => a.IsDeleted == false && a.MenuName == keyName.Trim());
            else
                count = _menuConfigRepository.GetCount(a => a.IsDeleted == false && a.MenuName == keyName.Trim() && a.Id != id);
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
