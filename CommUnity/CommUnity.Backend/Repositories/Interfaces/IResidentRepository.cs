using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface IResidentRepository
    {

        Task<ActionResponse<IEnumerable<User>>> GetResidentAsync(int apartmentId);

    }
}
