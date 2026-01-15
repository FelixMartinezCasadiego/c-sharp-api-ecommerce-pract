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
    RoleManager<IdentityRole> roleManager,
    IMapper mapper
) : IUserRepository
{
    private readonly ApplicationDbContext _db = db;
    private readonly string? secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
    private readonly UserManager<ApplicationUser> _userManager = userManager; 
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IMapper _mapper = mapper;

    public ApplicationUser? GetUser(string id)
    {
        return _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
    }

    public ICollection<ApplicationUser> GetUsers()
    {
        return _db.ApplicationUsers.OrderBy(u => u.UserName).ToList();
    }

    public bool IsUniqueUser(string username)
    {
        return !_db.ApplicationUsers.Any(u => u.UserName.ToLower().Trim() == username.ToLower().Trim());   
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

    public async Task<UserDataDto> Register(CreateUserDto createUserDto)
    {

        if (string.IsNullOrEmpty(createUserDto.Username))
        {
            throw new ArgumentException("Invalid username is required");
        }

        if (createUserDto.Password == null)
        {
            throw new ArgumentException("Invalid password is required");
        }

        var user = new ApplicationUser
        {
            UserName = createUserDto.Username,
            Email = createUserDto.Username,
            NormalizedEmail = createUserDto.Username.ToUpper(),
            Name = createUserDto.Name
        };

        var result = await _userManager.CreateAsync(user, createUserDto.Password);
        if (result.Succeeded)
        {
            var userRole = createUserDto.Role ?? "User";

            var roleExists = await _roleManager.RoleExistsAsync(userRole);
            if (!roleExists)
            {
                var identityRole = new IdentityRole(userRole);
                await _roleManager.CreateAsync(identityRole);
            }

            await _userManager.AddToRoleAsync(user, userRole);
            
            var createdUser = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == createUserDto.Username);

            return _mapper.Map<UserDataDto>(createdUser);
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new Exception("User registration failed: " + errors);

        // var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        // var user = new User
        // {
        //     Name = createUserDto.Name,
        //     Username = createUserDto.Username ?? "No Username",
        //     Password = encriptedPassword,
        //     Role = createUserDto.Role
        // };
        
        // await _db.Users.AddAsync(user);
        // await _db.SaveChangesAsync();
        // return user;
    }
}
