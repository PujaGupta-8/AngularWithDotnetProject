using AutoMapper;
using Neosoft_puja_backend.BAL.Interfaces;
using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;
using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.BAL.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepo _stateRepo;
        private readonly IMapper _mapper;
        public StateService(IMapper mapper, IStateRepo stateRepo)
        {
            _mapper = mapper;
            _stateRepo = stateRepo;
        }

        public async Task<IEnumerable<StateModel>> GetAllStateByCountryId(int id)
        {
            List<State> result =  (List<State>) await _stateRepo.GetAllByConditionAsync(a=>a.CountryId==id);
            return _mapper.Map<List<StateModel>>(result);
        }

        public  async Task<StateModel> GetStateStateId(int StateId)
        {
            State state= await _stateRepo.GetSingleAysnc(a => a.RowId == StateId);
            return _mapper.Map<StateModel>(state);
        }
    }


}
