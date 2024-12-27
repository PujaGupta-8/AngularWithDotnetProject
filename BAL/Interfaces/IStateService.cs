using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.BAL.Interfaces
{
    public interface IStateService
    {
        Task<IEnumerable<StateModel>> GetAllStateByCountryId(int id);
        Task<StateModel> GetStateStateId(int StateId);
    }
}
