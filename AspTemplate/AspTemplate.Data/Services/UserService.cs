using System.Security.Claims;
using AspTemplate.Core.Dto;
using AspTemplate.Core.Extensions;
using AspTemplate.Core.Model.Auth;
using AspTemplate.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using AspTemplate.Core.Configurations;
using AspTemplate.Core.Dto.Auth;
using AspTemplate.Core.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace AspTemplate.Data.Services;

public class UserService
{
    private readonly IEfCoreRepository<User> _userRepository;
    private readonly IEfCoreRepository<Role> _roleRepository;

    public UserService(IEfCoreRepository<User> userRepository, IEfCoreRepository<Role> roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    /// <param name="registerDto">данные для регистрации</param>
    /// <returns></returns>
    /// <exception cref="EntityExistsException">если пользователь с таким email уже существует</exception>
    /// <exception cref="ApplicationException">ошибка при создании пользователя</exception>
    public ResponseDto RegisterUser(RegisterDto registerDto)
    {
        if (_userRepository.Any(u => u.Email == registerDto.Email))
            throw new EntityExistsException(typeof(User), registerDto.Email);

        var passwordHash = registerDto.Password.Hash();
        var defaultRole = _roleRepository.GetListQuery()
            .AsTracking()
            .FirstOrDefault(r => r.Name == ApplicationConstants.DEFAULT_ROLE_NAME);
        
        if (defaultRole == null)
        {
            CreateDefaultRole();
            defaultRole = _roleRepository.GetListQuery()
                .AsTracking()
                .FirstOrDefault(r => r.Name == ApplicationConstants.DEFAULT_ROLE_NAME);        }

        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            UserRoles = new List<Role> { defaultRole! }
        };

        _userRepository.Add(user);
        _userRepository.SaveChanges();

        var token = GetToken(user.Email, registerDto.Password);
        if (token == null)
        {
            throw new ApplicationException("Error while creating a user");
        }

        return new ResponseDto
        {
            Email = user.Email,
            AccessToken = token,
            RoleIds = new List<int> { defaultRole.Id }
        };
    }

    /// <summary>
    /// Предоставить доступ для зарегистрированного пользователя
    /// </summary>
    /// <param name="loginDto">данные для входа</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException">если пользователя с таким email не существует</exception>
    /// <exception cref="AuthenticationException">неверные данные авторизации</exception>
    public ResponseDto LoginUser(LoginDto loginDto)
    {
        var user = _userRepository.GetListQuery()
            .Include(u => u.UserRoles)
            .FirstOrDefault(u => u.Email == loginDto.Email);

        if (user == null)
        {
            throw new EntityNotFoundException(typeof(User), loginDto.Email);
        }

        if (loginDto.Password.Hash() != user.PasswordHash)
        {
            throw new AuthenticationException("Wrong password");
        }

        return new ResponseDto
        {
            Email = user.Email,
            AccessToken = GetToken(loginDto.Email, loginDto.Password)!,
            RoleIds = user.UserRoles.Select(r => r.Id).ToList()
        };
    }

    private void CreateDefaultRole()
    {
        var newRole = new Role
        {
            Name = ApplicationConstants.DEFAULT_ROLE_NAME,
            Description = "Учетная запись обычного пользователя"
        };
        _roleRepository.Add(newRole);
        _roleRepository.SaveChanges();
    }

    private ClaimsIdentity? GetIdentity(string email, string password)
    {
        var passwordHash = password.Hash();

        // Информация о пользователе
        var user = _userRepository.GetListQuery()
            .Include(p => p.UserRoles)
            .FirstOrDefault(x => x.Email == email && x.PasswordHash == passwordHash);

        // Если пользователя нет
        if (user == null)
            return null;

        // Параметры токена
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim("id", user.Id.ToString())
        };

        // Добавить роли в токен
        const string typeRole = "Role";

        user.UserRoles.Select(p => p.Name)
            .ToList()
            .ForEach(p => claims.Add(new Claim(typeRole, p)));

        ClaimsIdentity claimsIdentity = new(claims, "Token", ClaimsIdentity.DefaultNameClaimType, typeRole);

        return claimsIdentity;
    }


    private string? GetToken(string email, string password)
    {
        var identity = GetIdentity(email, password);

        if (identity == null)
            return null;

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}