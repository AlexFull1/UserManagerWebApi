using WebAPIProject.Models;
using WebAPIProject.DTOs;
using WebAPIProject.DTOs.UsersDTO;

namespace WebAPIProject.Services
{
    public interface IUserServices
    {
        Task<List<GetUserDTO>> GetAllUsers();
        Task<GetUserDTO> GetUserById(int id);
        Task<GetUserDTO> AddUser(AddUserDTO user);
        Task UpdateUser(UpdateUserDTO updateUserDTO, int id);
        Task DeleteUser(int id);
    }
}
