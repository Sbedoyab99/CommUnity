using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Interfaces
{
    public interface IResidentUnitOfWork
    {
        //Task<ActionResponse<User>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<User>>> GetResidentAsync(int apartmentId);

    }
}
