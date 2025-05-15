using AutoMapper;
using Warehouse.Common.Responses;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using Warehouse.Data.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.PasswordServices;
using Warehouse.Services.Services; // Importa il namespace di PasswordService
using System;

namespace Warehouse.Service.Services
{
  public class UserService : GenericService<Users, DTOUser>, IUserService
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly PasswordService _passwordService; // Campo per PasswordService

    // Modifica il costruttore per accettare PasswordService
    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger, PasswordService passwordService)
        : base(userRepository, mapper, logger)
    {
      _userRepository = userRepository;
      _mapper = mapper;
      _logger = logger;
      _passwordService = passwordService;
    }

    // Override del metodo UpdateAsync per fornire un'implementazione specifica per Users e DTOUser
    public override async Task<BaseResponse<DTOUser>> UpdateAsync(int id, DTOUser dto)
    {
      _logger.LogInformation($"UserService: Aggiornamento utente con ID {id}");
      var existingUser = await _userRepository.GetByIdAsync(id);
      if (existingUser == null)
      {
        _logger.LogWarning($"UserService: Utente con ID {id} non trovato per l'aggiornamento.");
        // **Modifica qui:** Usa NotFoundResponse come già stai facendo, ma potresti voler passare un codice di stato
        return BaseResponse<DTOUser>.NotFoundResponse($"Utente con ID {id} non trovato.", 404);
      }

      // Mappa i valori dal DTOUser all'oggetto Users esistente
      _mapper.Map(dto, existingUser);

      // Esegui l'aggiornamento tramite il repository
      await _userRepository.UpdateAsync(existingUser);

      // Mappa l'entità Users aggiornata a un DTOUser per la risposta
      var updatedDto = _mapper.Map<DTOUser>(existingUser);
      _logger.LogInformation($"UserService: Utente con ID {id} aggiornato con successo.");
      return BaseResponse<DTOUser>.SuccessResponse(updatedDto, "Utente aggiornato con successo.");
    }

    public async Task<DTOUser> GetByEmailAsync(string email)
    {
      _logger.LogInformation($"UserService: Recupero utente con email {email}");
      var user = await _userRepository.GetByEmailAsync(email); // Get Users from repository
      return _mapper.Map<DTOUser>(user); // Map Users to DTOUser
    }

    public Task<BaseResponse<DTOUser>> CreateAsync(DTOCreateUser createUserDto)
    {
      return CreateAsync(createUserDto, DateTime.UtcNow);
    }

    public async Task<BaseResponse<DTOUser>> CreateAsync(DTOCreateUser createUserDto, DateTime dateTime)
    {
      _logger.LogInformation($"UserService: Tentativo di creazione utente con email {createUserDto.Email}");

      // **Controllo esistenza utente tramite email**
      var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
      if (existingUser != null)
      {
        _logger.LogWarning($"UserService: Utente con email {createUserDto.Email} già esistente.");
        return BaseResponse<DTOUser>.ErrorResponse($"L'email '{createUserDto.Email}' è già registrata.", 409);
      }

      // Mappa da DTOCreateUser a Users (Domain Model)
      var user = _mapper.Map<Users>(createUserDto);

      // Hashing della password
      if (!string.IsNullOrEmpty(createUserDto.Password))
      {
        user.PasswordHash = _passwordService.HashPassword(createUserDto.Password);
      }
      else
      {
        _logger.LogWarning($"UserService: Password non fornita per l'utente con email {createUserDto.Email}");
        return BaseResponse<DTOUser>.ErrorResponse("La password è obbligatoria.", 400);
      }

      // Imposta la data di creazione in UTC
      user.CreatedAt = dateTime;

      // Use the repository to add the user to the database
      await _userRepository.AddAsync(user);

      // Mappa l'entità Users creata a un DTOUser per la risposta
      var createdDto = _mapper.Map<DTOUser>(user);

      _logger.LogInformation($"UserService: Utente con email {createUserDto.Email} creato con successo");
      return BaseResponse<DTOUser>.SuccessResponse(createdDto, "Utente creato con successo.");
    }

  }
}
