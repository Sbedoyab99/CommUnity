﻿using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface IUsersRepository
    {

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);


        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserAsync(string email);



        Task<User> GetUserAsync(Guid userId);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginDTO model);

        Task LogoutAsync();

        Task<ActionResponse<IEnumerable<User>>> GetUsersAsync(PaginationDTO pagination, UserType role);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination, UserType role);

        Task<ActionResponse<int>> GetRecordsNumber(PaginationDTO pagination, UserType role);

        Task<ActionResponse<User>> GetAdminResidentialUnit(int residentialUnitId);

    }
}
