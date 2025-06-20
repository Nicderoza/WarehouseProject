using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using AutoMapper; // Aggiungi AutoMapper se lo usi per mappare User a DTOUser

namespace Warehouse.Service.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<Users> _passwordHasher;
    private readonly IJWTService _jwtService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMapper _mapper; // Aggiungi IMapper

    public AuthenticationService(IUserRepository userRepository, IPasswordHasher<Users> passwordHasher, IJWTService jwtService, IConfiguration configuration, ILogger<AuthenticationService> logger, IMapper mapper) // Inietta IMapper
    {
      _userRepository = userRepository;
      _passwordHasher = passwordHasher;
      _jwtService = jwtService;
      _configuration = configuration;
      _logger = logger;
      _mapper = mapper; 
    }

    public async Task<BaseResponse<DTOLoginSuccessResponse>> LoginAsync(DTOLogin loginDTO) 
    {
      _logger.LogInformation($"Attempting login for user with email: {loginDTO.Email}");

      try
      {
        if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
        {
          _logger.LogWarning("Login request is invalid (null or missing email/password).");
          return BaseResponse<DTOLoginSuccessResponse>.ErrorResponse("Credenziali di accesso non valide.", (int)HttpStatusCode.BadRequest);
        }

        var user = await _userRepository.GetByEmailAsync(loginDTO.Email);

        if (user == null)
        {
          _logger.LogWarning($"User with email: {loginDTO.Email} not found.");
          return BaseResponse<DTOLoginSuccessResponse>.InvalidEmail();
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);

        if (verificationResult != PasswordVerificationResult.Success)
        {
          _logger.LogWarning($"Invalid password for user with email: {loginDTO.Email}.");
          return BaseResponse<DTOLoginSuccessResponse>.InvalidPassword(); 
        }

        string token = _jwtService.GenerateToken(user);
        _logger.LogInformation($"User with email: {loginDTO.Email} logged in successfully. Token generated.");

        
        var userDto = _mapper.Map<DTOUser>(user);

       
        var loginSuccessResponse = new DTOLoginSuccessResponse
        {
          Token = token,
          User = userDto
        };

        return BaseResponse<DTOLoginSuccessResponse>.SuccessResponse(loginSuccessResponse); 
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error during login for user with email: {loginDTO.Email}");
        return BaseResponse<DTOLoginSuccessResponse>.ErrorResponse($"Errore interno durante il login: {ex.Message}.", (int)HttpStatusCode.InternalServerError);
      }
    }

    public string HashPassword(Users user, string password)
    {
      return _passwordHasher.HashPassword(user, password);
    }

    public string HashNewPassword(string newPassword)
    {
      return _passwordHasher.HashPassword(null, newPassword);
    }

    public PasswordVerificationResult VerifyPassword(Users user, string hashedPassword, string providedPassword)
    {
      return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
    }
  }
}

/*
   "userID": 5,
        "roleID": 0,
        "name": "Michael",
        "surname": "Brown",
        "email": "michael.brown@example.com",
        "passwordHash": "y5Z6a7B8c9D0",
        "birthDate": "1988-03-10T00:00:00",
        "createdAt": "2026-01-28T00:00:00"
*/
