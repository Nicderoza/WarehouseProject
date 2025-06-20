using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System; 

namespace Warehouse.Service.Services;

public class UserService : GenericService<Users, DTOUser>, IUserService
{
  private readonly IUserRepository _userRepository;
  private readonly IMapper _mapper;
  private readonly ILogger<UserService> _logger;
  private readonly IAuthenticationService _authenticationService;

  public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger, IAuthenticationService authenticationService)
      : base(userRepository, mapper, logger)
  {
    _userRepository = userRepository;
    _mapper = mapper;
    _logger = logger;
    _authenticationService = authenticationService;
  }

  public async Task<BaseResponse<DTOUser>> GetUserByIdAsync(int id)
  {
    _logger.LogInformation($"UserService: Recupero entità utente con ID {id}");
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
    {
      _logger.LogWarning($"UserService: Utente con ID {id} non trovato.");
      return BaseResponse<DTOUser>.NotFoundResponse("Utente non trovato.", (int)HttpStatusCode.NotFound);
    }
    var userDto = _mapper.Map<DTOUser>(user);
    return BaseResponse<DTOUser>.SuccessResponse(userDto, "Utente trovato.", (int)HttpStatusCode.OK);
  }

  public async Task<BaseResponse<DTOUser>> UpdateUserAsync(int id, DTOUser userDto)
  {
    _logger.LogInformation($"UserService: Aggiornamento utente con ID {id} (DTOUser)");
    var existingUser = await _userRepository.GetByIdAsync(id);
    if (existingUser == null)
    {
      _logger.LogWarning($"UserService: Utente con ID {id} non trovato per l'aggiornamento.");
      return BaseResponse<DTOUser>.NotFoundResponse("Utente non trovato per l'aggiornamento.", (int)HttpStatusCode.NotFound);
    }

    _mapper.Map(userDto, existingUser);
    existingUser.UserID = id;

    await _userRepository.UpdateAsync(existingUser);
    var updatedDto = _mapper.Map<DTOUser>(existingUser);
    _logger.LogInformation($"UserService: Utente con ID {id} aggiornato con successo.");
    return BaseResponse<DTOUser>.SuccessResponse(updatedDto, "Utente aggiornato con successo.", (int)HttpStatusCode.OK);
  }

  public async Task<BaseResponse<DTOUser>> CreateAsync(DTOCreateUser createUserDto)
  {
    _logger.LogInformation($"UserService: Tentativo di creazione utente con email {createUserDto.Email}");

    var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
    if (existingUser != null)
    {
      _logger.LogWarning($"UserService: Utente con email {createUserDto.Email} già esistente.");
      return BaseResponse<DTOUser>.ErrorResponse("Profilo già esistente.", (int)HttpStatusCode.Conflict);
    }

    var user = _mapper.Map<Users>(createUserDto);

    if (!string.IsNullOrEmpty(createUserDto.Password))
    {
      user.PasswordHash = _authenticationService.HashPassword(user, createUserDto.Password);
    }
    else
    {
      _logger.LogWarning($"UserService: Password non fornita per l'utente con email {createUserDto.Email}");
      return BaseResponse<DTOUser>.ErrorResponse("La password è obbligatoria.", (int)HttpStatusCode.BadRequest);
    }

    user.CreatedAt = DateTime.UtcNow; 

    await _userRepository.AddAsync(user);
    var createdDto = _mapper.Map<DTOUser>(user);
    _logger.LogInformation($"UserService: Utente con email {createUserDto.Email} creato con successo");
    return BaseResponse<DTOUser>.SuccessResponse(createdDto, "Utente creato con successo.", (int)HttpStatusCode.Created);
  }

  public async Task<DTOUser> GetByEmailAsync(string email)
  {
    _logger.LogInformation($"UserService: Recupero utente con email {email}");
    var user = await _userRepository.GetByEmailAsync(email);
    return _mapper.Map<DTOUser>(user);
  }

  public async Task<BaseResponse<string>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
  {
    _logger.LogInformation($"UserService: Tentativo di cambio password per utente con ID {userId}");

    try
    {
      var user = await _userRepository.GetByIdAsync(userId);
      if (user == null)
      {
        _logger.LogWarning($"UserService: Utente con ID {userId} non trovato per il cambio password.");
        return BaseResponse<string>.NotFoundResponse("Utente non trovato.", (int)HttpStatusCode.NotFound);
      }

      var verificationResult = _authenticationService.VerifyPassword(user, user.PasswordHash, currentPassword);

      if (verificationResult != PasswordVerificationResult.Success)
      {
        _logger.LogWarning($"UserService: Password corrente non valida per utente con ID {userId}.");
        return BaseResponse<string>.InvalidPassword("Password corrente non valida.", (int)HttpStatusCode.Unauthorized);
      }

      user.PasswordHash = _authenticationService.HashNewPassword(newPassword);

      await _userRepository.UpdateAsync(user);
      _logger.LogInformation($"UserService: Password per utente con ID {userId} cambiata con successo.");
      return BaseResponse<string>.SuccessResponse("Password cambiata con successo.", null, (int)HttpStatusCode.OK);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, $"UserService: Errore durante il cambio password per utente con ID {userId}");
      return BaseResponse<string>.ErrorResponse($"Errore interno durante il cambio password: {ex.Message}.", (int)HttpStatusCode.InternalServerError);
    }
  }
  public async Task<DTOUser> ToggleStatusAsync(int userId)
  {
    var user = await _userRepository.ToggleStatusAsync(userId);
    return _mapper.Map<DTOUser>(user);
  }

  public Task AssociateSupplierAsync(int userId, int supplierId)
      => _userRepository.AssociateSupplierAsync(userId, supplierId);

  public async Task DissociateSupplierAsync(int userId, int supplierId)
  {
    await _userRepository.DissociateSupplierAsync(userId, supplierId);
  }


  public Task ChangeRoleAsync(int userId, string newRole)
      => _userRepository.ChangeRoleAsync(userId, newRole);

  public async Task<List<DTOUser>> GetUsersWithoutSupplierAsync()
  {
    var users = await _userRepository.GetUsersWithoutSupplierAsync();
    return _mapper.Map<List<DTOUser>>(users);
  }

  public async Task<List<DTOUser>> GetUsersWithSuppliersAsync()
  {
    var users = await _userRepository.GetUsersWithSuppliersAsync();
    return _mapper.Map<List<DTOUser>>(users);
  }
  public async Task<List<DTOUser>> GetUsersBySupplierAsync(int supplierId)
  {
    var users = await _userRepository.GetUsersBySupplierAsync(supplierId);
    return _mapper.Map<List<DTOUser>>(users);
  }

}
