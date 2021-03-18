using System.Collections.Generic;
using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;
using System.Linq;
using System;

namespace UPTax.Data.Repository
{
    public interface IMenuPermissionRepository : IRepository<MenuPermission>
    {
        List<MenuPermission> GetAllPermittedMenues(string roleId, int categoryId);
        bool DeleteAllPermittedMenues(string roleId, int categoryId);
    }
    public class MenuPermissionRepository : Repository<MenuPermission>, IMenuPermissionRepository
    {
        AdminContext _context;
        public MenuPermissionRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }

        public bool DeleteAllPermittedMenues(string roleId, int categoryId)
        {
            List<MenuPermission> result = (from mp in _context.MenuPermissions join m in _context.MenuConfigs on mp.MenuConfigId equals m.Id where mp.RoleId.Equals(roleId) && m.CategoryId == categoryId select mp).ToList();
            try
            {
                _context.MenuPermissions.RemoveRange(result);
                if (result.Count > 0)
                {
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                return false;
            }
        }

        public List<MenuPermission> GetAllPermittedMenues(string roleId, int categoryId)
        {
            List<MenuPermission> result = (from mp in _context.MenuPermissions join m in _context.MenuConfigs on mp.MenuConfigId equals m.Id where mp.RoleId.Equals(roleId) && m.CategoryId == categoryId select mp).ToList();
            return result;
        }
    }
}
