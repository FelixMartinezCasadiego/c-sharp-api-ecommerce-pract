using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiEcommerce.Repository;

public class UserRepository(
    ApplicationDbContext db, 
    IConfiguration configuration, 
    UserManager<ApplicationUser> userManager,
    UserManager<IdentityRole> roleManager,
    IMapper mapper
) : IUserRepository
{
    private readonly ApplicationDbContext _db = db;
    private readonly string? secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
    private readonly UserManager<ApplicationUser> _userManager = userManager; 
    private readonly UserManager<IdentityRole> _roleManager = roleManager;
    private readonly IMapper _mapper = mapper;

    public User? GetUser(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }

    public ICollection<User> GetUsers()
    {
        return _db.Users.OrderBy(u => u.Username).ToList();
    }

    public bool IsUniqueUser(string username)
    {
        return !_db.Users.Any(u => u.Username.ToLower().Trim() == username.ToLower().Trim());   
    }

    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        if (string.IsNullOrEmpty(userLoginDto.Username))
        {
            return new UserLoginResponseDto { Token = "", User = null, Message = "Invalid username is required" };
        }

        var user = await _db.ApplicationUsers.FirstOrDefaultAsync<ApplicationUser>(user => user.UserName != null && user.UserName.ToLower().Trim() == userLoginDto.Username.ToLower().Trim());

        if (user == null)
        {
            return new UserLoginResponseDto { Token = "", User = null, Message = "Invalid username" };
        }

        if (userLoginDto.Password == null)
        {
            return new UserLoginResponseDto { Token = "", User = null, Message = "Invalid password is required" };
        }

        bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        // if(!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
        if(!isValid)
        {
            return new UserLoginResponseDto { Token = "", User = null, Message = "Invalid password" };
        }

        // JWT Token Generation
        var handlerToken = new JwtSecurityTokenHandler();

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException("Secret key is not configured.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var key = Encoding.UTF8.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
            ]),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handlerToken.CreateToken(tokenDescriptor);

        return new UserLoginResponseDto
        {
            Token = handlerToken.WriteToken(token),
            // User = new UserRegisterDto()
            User = _mapper.Map<UserDataDto>(user),
            Message = "Login successful"
        };
    }

    public async Task<User> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var user = new User
        {
            Name = createUserDto.Name,
            Username = createUserDto.Username ?? "No Username",
            Password = encriptedPassword,
            Role = createUserDto.Role
        };
        
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return user;
    }
}
