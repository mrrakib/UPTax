using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Repository;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services
{
    public interface IGenderService
    {
        List<Gender> GetAll();
        List<IdNameDropdown> GetDropdownItemList();
    }
    class GenderService : IGenderService
    {
        private readonly IGenderRepository _genderRepository;

        public GenderService(IGenderRepository genderRepository)
        {
            _genderRepository = genderRepository;
        }
        public List<Gender> GetAll()
        {
            return _genderRepository.GetMany(a => a.IsDeleted == false).ToList();
        }
        public List<IdNameDropdown> GetDropdownItemList()
        {
            return _genderRepository.GetMany(w => w.IsDeleted == false).Select(u => new IdNameDropdown
            {
                Id = u.Id,
                Name = u.Name
            }).ToList();
        }
    }
}
