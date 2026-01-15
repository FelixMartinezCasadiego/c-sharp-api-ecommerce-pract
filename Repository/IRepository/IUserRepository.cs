using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;

namespace ApiEcommerce.Repository.IRepository;

public interface IUserRepository
{
    ICollection<ApplicationUser> GetUsers();
    ApplicationUser? GetUser(string id);
    bool IsUniqueUser(string username);
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<UserDataDto> Register(CreateUserDto createUserDto);
}

// Task representa una operación asíncrona en C#.
// Task<T> indica que la operación devolverá un resultado de tipo T cuando termine.
// Por ejemplo, Task<UserLoginResponseDto> significa que el método es asíncrono y, al completarse, devolverá un objeto UserLoginResponseDto.