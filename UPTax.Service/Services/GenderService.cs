using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface IGenderService
    {
        List<Gender> GetAll();
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
    }
}
