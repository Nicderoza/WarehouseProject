using AutoMapper;
using Warehouse.Common.Responses;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;  // Ensure you are using this for logging

namespace Warehouse.Services.Services
{
    public abstract class GenericService<TEntity, TDTO> : IGenericService<TDTO>
        where TEntity : class
        where TDTO : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        private readonly ILogger<GenericService<TEntity, TDTO>> _logger;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper, ILogger<GenericService<TEntity, TDTO>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public virtual async Task<BaseResponse<IEnumerable<TDTO>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all entities of type {EntityType}", typeof(TEntity).Name); // Log entity type being fetched
            var entities = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<TDTO>>(entities);
            _logger.LogInformation("Successfully fetched {EntityCount} entities of type {EntityType}", entities.Count(), typeof(TEntity).Name); // Log number of entities fetched
            return BaseResponse<IEnumerable<TDTO>>.SuccessResponse(dtos, "Elenco recuperato con successo.");
        }

        public virtual async Task<BaseResponse<TDTO>> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id); // Log entity ID being fetched
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Entity of type {EntityType} with ID {EntityId} not found", typeof(TEntity).Name, id); // Log warning if entity not found
                return BaseResponse<TDTO>.NotFoundResponse($"Entità con ID {id} non trovata.");
            }
            var dto = _mapper.Map<TDTO>(entity);
            _logger.LogInformation("Successfully fetched entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id); // Log successful fetch
            return BaseResponse<TDTO>.SuccessResponse(dto, "Entità recuperata con successo.");
        }
    
        public virtual async Task<BaseResponse<TDTO>> AddAsync(TDTO dto)
        {
            _logger.LogInformation("Adding new entity of type {EntityType}", typeof(TEntity).Name); // Log entity type being added
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            var resultDto = _mapper.Map<TDTO>(entity);
            _logger.LogInformation("Successfully added new entity of type {EntityType}", typeof(TEntity).Name); // Log successful addition
            return BaseResponse<TDTO>.SuccessResponse(resultDto, "Entità aggiunta con successo.");
        }

        public virtual async Task<BaseResponse<TDTO>> UpdateAsync(int id, TDTO dto)
        {
            _logger.LogInformation("Updating entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id); // Log entity ID being updated
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                _logger.LogWarning("Entity of type {EntityType} with ID {EntityId} not found for update", typeof(TEntity).Name, id); // Log warning if entity not found
                return BaseResponse<TDTO>.NotFoundResponse($"Impossibile trovare l'entità con ID {id} da aggiornare.");
            }
            _mapper.Map(dto, existingEntity);
            await _repository.UpdateAsync(existingEntity);
            var updatedDto = _mapper.Map<TDTO>(existingEntity);
            _logger.LogInformation("Successfully updated entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id); // Log successful update
            return BaseResponse<TDTO>.SuccessResponse(updatedDto, "Entità aggiornata con successo.");
        }

        public virtual async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id); // Log entity ID being deleted
            var entityToDelete = await _repository.GetByIdAsync(id);
            if (entityToDelete == null)
            {
                _logger.LogWarning("Entity of type {EntityType} with ID {EntityId} not found for deletion", typeof(TEntity).Name, id); // Log warning if entity not found
                return BaseResponse<bool>.NotFoundResponse($"Impossibile trovare l'entità con ID {id} da eliminare.");
            }
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Successfully deleted entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id); // Log successful deletion
            return BaseResponse<bool>.SuccessResponse(true, "Entità eliminata con successo.");
        }
     
    }
}
