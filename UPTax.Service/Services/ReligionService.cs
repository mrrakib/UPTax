using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Repository;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services
{
    public interface IReligionService
    {
        List<Religion> GetAll();
        List<IdNameDropdown> GetDropdownItemList();
    }
    class ReligionService : IReligionService
    {
        private readonly IReligionRepository _religionRepository;

        public ReligionService(IReligionRepository religionRepository)
        {
            _religionRepository = religionRepository;
        }
        public List<Religion> GetAll()
        {
            return _religionRepository.GetMany(a => a.IsDeleted == false).ToList();
        }
        public List<IdNameDropdown> GetDropdownItemList()
        {
            return _religionRepository.GetMany(w => w.IsDeleted == false).Select(u => new IdNameDropdown
            {
                Id = u.Id,
                Name = u.Name
            }).ToList();
        }
    }
}
