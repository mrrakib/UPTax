using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface IReligionService
    {
        List<Religion> GetAll();
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
    }
}
