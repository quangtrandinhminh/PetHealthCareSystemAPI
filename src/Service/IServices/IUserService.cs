﻿using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;

namespace Service.IServices;

public interface IUserService
{
    Task CreateVetAsync(VetRequestDto dto);
    Task<IList<UserResponseDto>> GetAllUsersByRoleAsync(int role);
    Task CreateUserAsync(UserCreateRequestDto dto);
    Task UpdateUserAsync(UserUpdateRequestDto dto);
    Task<UserResponseDto> GetByIdAsync(int id);
    Task DeleteUserAsync(int id);
    Task<UserResponseDto> GetVetByIdAsync(int id);
}