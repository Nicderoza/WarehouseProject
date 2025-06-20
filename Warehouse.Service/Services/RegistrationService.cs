// Warehouse.Service.Services/RegistrationService.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Data.Interfaces; 
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System; // Aggiungi questo using se non presente per DateTime.UtcNow
using System.Threading.Tasks; // Aggiungi questo using per Task

namespace Warehouse.Service.Services
{
  public class RegistrationService : IRegistrationService
  {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<Users> _passwordHasher;
    private readonly ILogger<RegistrationService> _logger;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUsersSuppliersRepository _usersSuppliersRepository;

    public RegistrationService(IUserRepository userRepository, IPasswordHasher<Users> passwordHasher,
                                ILogger<RegistrationService> logger, ISupplierRepository supplierRepository,
                                IUsersSuppliersRepository usersSuppliersRepository)
    {
      _userRepository = userRepository;
      _passwordHasher = passwordHasher;
      _logger = logger;
      _supplierRepository = supplierRepository;
      _usersSuppliersRepository = usersSuppliersRepository;
    }

    public async Task<bool> RegisterUser(DTOCreateUser userDto)
    {
      _logger.LogInformation($"Tentativo di registrazione utente 'normale' (acquirente) con email: {userDto.Email}");

      if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Name) ||
          string.IsNullOrEmpty(userDto.Surname) || string.IsNullOrEmpty(userDto.Password))
      {
        _logger.LogWarning("Validazione registrazione utente (acquirente) fallita: campi obbligatori mancanti.");
        return false;
      }

      if (await _userRepository.GetByEmailAsync(userDto.Email) != null)
      {
        _logger.LogWarning($"Registrazione utente (acquirente) fallita: L'email '{userDto.Email}' è già in uso.");
        return false;
      }

      var newUser = new Users
      {
        Email = userDto.Email,
        Name = userDto.Name,
        Surname = userDto.Surname,
        BirthDate = userDto.BirthDate.Value,
        RoleID = 3, 
        CreatedAt = DateTime.UtcNow
      };

      if (!string.IsNullOrEmpty(userDto.Password))
      {
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, userDto.Password);
      }
      else
      {
        _logger.LogWarning($"Registrazione utente (acquirente) fallita: Password non fornita per l'utente {userDto.Email}.");
        return false;
      }

      await _userRepository.AddAsync(newUser);

      if (await _userRepository.SaveChangesAsync() <= 0)
      {
        _logger.LogError($"Fallimento nel salvare l'utente '{userDto.Email}' nel database.");
        return false;
      }

      _logger.LogInformation($"Utente (acquirente) '{userDto.Email}' registrato con successo con RoleID: {newUser.RoleID}. UserID: {newUser.UserID}");
      return true;
    }

    public async Task<bool> RegisterSupplier(DTORegisterSupplier dto)
    {
      _logger.LogInformation($"Tentativo di registrazione fornitore (e suo gestore) con email: {dto.Email}");

      if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password) ||
          string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.LastName) ||
          string.IsNullOrEmpty(dto.CompanyName))
      {
        _logger.LogWarning("Validazione registrazione fornitore fallita: campi obbligatori mancanti o non validi.");
        return false;
      }

      if (await _userRepository.GetByEmailAsync(dto.Email) != null)
      {
        _logger.LogWarning($"Registrazione fornitore fallita: L'email del gestore '{dto.Email}' è già in uso.");
        return false;
      }

      if (await _supplierRepository.GetByNameAsync(dto.CompanyName) != null)
      {
        _logger.LogWarning($"Registrazione fornitore fallita: Il nome dell'azienda '{dto.CompanyName}' è già in uso.");
        return false;
      }

      var newSupplierManagerUser = new Users
      {
        Email = dto.Email,
        Name = dto.FirstName,
        Surname = dto.LastName,
        BirthDate = dto.BirthDate.GetValueOrDefault(),
        RoleID = 2,
        CreatedAt = DateTime.UtcNow
      };

      newSupplierManagerUser.PasswordHash = _passwordHasher.HashPassword(newSupplierManagerUser, dto.Password);

      await _userRepository.AddAsync(newSupplierManagerUser);
      if (await _userRepository.SaveChangesAsync() <= 0)
      {
        _logger.LogError($"Fallimento nel salvare l'utente gestore fornitore '{dto.Email}' nel database.");
        return false;
      }
      _logger.LogInformation($"Account utente gestore fornitore '{dto.Email}' creato con successo. UserID: {newSupplierManagerUser.UserID}");

      var newSupplier = new Suppliers
      {
        CompanyName = dto.CompanyName
      };

      if (dto.CityID.HasValue) 
      {
        newSupplier.CityID = dto.CityID.Value;
      }

      await _supplierRepository.AddAsync(newSupplier);
      if (await _supplierRepository.SaveChangesAsync() <= 0)
      {
        _logger.LogError($"Fallimento nel salvare i dettagli del fornitore '{dto.CompanyName}' nel database.");
        return false;
      }
      _logger.LogInformation($"Dettagli fornitore '{dto.CompanyName}' salvati con successo. SupplierID: {newSupplier.SupplierID}");

      var userSupplierAssociation = new UsersSuppliers
      {
        UserID = newSupplierManagerUser.UserID,
        SupplierID = newSupplier.SupplierID
      };

      await _usersSuppliersRepository.AddUsersSuppliersAsync(userSupplierAssociation);
      _logger.LogInformation($"Associazione creata tra UserID: {newSupplierManagerUser.UserID} (gestore fornitore) e SupplierID: {newSupplier.SupplierID}.");

      return true;
    }
  }
}
