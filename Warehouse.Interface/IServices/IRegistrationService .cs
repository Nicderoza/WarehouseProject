using Warehouse.Common.DTOs;
using System.Threading.Tasks;

namespace Warehouse.Interfaces.IServices
{
  public interface IRegistrationService
  {
    /// <summary>
    /// Registra un nuovo utente con RoleID = 2 (User).
    /// </summary>
    /// <param name="userDto">Il DTO contenente i dati dell'utente da registrare.</param>
    /// <returns>True se la registrazione ha successo, altrimenti false.</returns>
    Task<bool> RegisterUser(DTOCreateUser userDto);

    /// <summary>
    /// Registra un nuovo fornitore, creando sia l'account utente (RoleID = 3) che i dettagli del fornitore.
    /// </summary>
    /// <param name="dto">Il DTO contenente i dati di registrazione del fornitore.</param>
    /// <returns>True se la registrazione ha successo, altrimenti false.</returns>
    Task<bool> RegisterSupplier(DTORegisterSupplier dto);
  }
}
