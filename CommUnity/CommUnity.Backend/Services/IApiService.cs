using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Services
{
    public interface IApiService
    {
        Task<ActionResponse<T>> GetAsync<T>(string servicePrefix, string controller);
    }
}