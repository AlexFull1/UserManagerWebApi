using WebAPIProject.DTOs.UsersDTO;
using WebAPIProject.Models;
using WebAPIProject.Repositories;


namespace WebAPIProject.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserDTO> AddUser(AddUserDTO userDTO)
        {
            var user = new User { Name = userDTO.Name, Age = userDTO.Age, Balance = userDTO.Balance };
            await _userRepository.AddAsync(user);
            
            GetUserDTO getUserDTO = await GetUserById(user.Id);
            return getUserDTO;
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            await _userRepository.DeleteAsync(user);
        }

        public async Task<List<GetUserDTO>> GetAllUsers() 
        {
            var allUsers = new List<GetUserDTO>();
            var users = await _userRepository.GetAllAsync();
            foreach (var user in users) 
            {
                allUsers.Add(new GetUserDTO()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Age = user.Age,
                    Balance = user.Balance
                });          
            }
            return allUsers;
        }

        public async Task<GetUserDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var getUser = new GetUserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Balance = user.Balance
            };
            
            if (getUser == null)
                throw new Exception("User Not Found");

            return getUser;
        }

        public async Task UpdateUser(UpdateUserDTO updateUserDTO, int id)
        {            
                var user = await _userRepository.GetByIdAsync(id);
                user.Name = updateUserDTO.Name;
                user.Age = updateUserDTO.Age;
                await _userRepository.UpdateAsync(user);           
        }
    }
}

