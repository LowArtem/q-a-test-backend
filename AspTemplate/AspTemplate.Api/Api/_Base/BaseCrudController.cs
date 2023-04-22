using AspTemplate.Api.Extensions.Api;
using AspTemplate.Core.Model._Base;
using AspTemplate.Core.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AspTemplate.Api.Api._Base;

/// <summary>
/// Базовый контроллер выполняющий CRUD операции
/// </summary>
[ApiController]
public abstract class BaseCrudController<T> : ControllerBase
    where T : IEntity
{
    /// <summary>
    /// Репозиторий
    /// </summary>
    private readonly IEfCoreRepository<T> _repository;

    private readonly ILogger<BaseCrudController<T>> _logger;
    private readonly IMapper _mapper;

    public BaseCrudController(IEfCoreRepository<T> repository, ILogger<BaseCrudController<T>> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Получение списка записей
    /// </summary>
    protected virtual IQueryable<T> List => _repository.GetListQuery();

    /// <summary>
    /// Получение списка всех записей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public virtual IActionResult ListEntities()
    {
        var result = List.OrderBy(p => p.DateCreate)
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Получение списка части записей
    /// </summary>
    /// <param name="limit">Количество записей в ответе (выбираются первые {limit} записей из выборки после {skip} элементов)
    /// <br/><br/>
    /// <b>Max value = 1000</b>
    /// </param>
    /// <param name="skip">Пропускается первых {skip} записей</param>
    /// <returns></returns>
    [HttpGet("piece")]
    public virtual IActionResult ListEntitiesPiece(int limit = 1000, int skip = 0)
    {
        if (limit is < 0 or > 1000) limit = 1000;
        if (skip < 0) skip = 0;

        var result = List.OrderBy(p => p.DateCreate)
            .Skip(skip)
            .Take(limit)
            .ToList();

        return Ok(result);
    }
    
    /// <summary>
    /// Получение записи
    /// </summary>
    /// <param name="id">Идентификатор записи</param>
    /// <returns>Запись</returns>
    /// <response code="404">Если записи с данным идентификатором не существует</response>   
    [HttpGet("{guid:guid}")]
    [SwaggerResponse(404, "Запись с таким идентификатором не существует")]
    public virtual IActionResult Get(int id)
    {
        var entity = List.FirstOrDefault(p => p.Id == id);

        if (entity == null) return NotFound("Id не найден");
        return Ok(entity);
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="model">Запись</param>
    /// <response code="400">Запись не прошла валидацию</response>
    /// <response code="500">При добавлении записи произошла ошибка на сервере</response>   
    /// <returns>Добавленная запись</returns>
    [HttpPost]
    [SwaggerResponse(200, "Запись успешно добавлена. Содержит информацию о добавленной записи", typeof(BaseEntity))]
    [SwaggerResponse(400, "Ошибка валидации")]
    [SwaggerResponse(500, "Ошибка при добавлении записи")]
    public virtual IActionResult Add(T model)
    {
        _repository.Add(model);
        _repository.SaveChanges();
        return model.Id > 0 ? Ok(model) : StatusCode(500, "Произошла ошибка при добавлении записи");
    }

    /// <summary>
    /// Добавление записей
    /// </summary>
    /// <param name="models">Записи</param>
    /// <response code="400">Записи не прошли валидацию</response>
    /// <response code="500">При добавлении записи произошла ошибка на сервере</response>   
    /// <returns>Добавленная запись</returns>
    [HttpPost("range")]
    [SwaggerResponse(200, "Записи успешно добавлены. Содержит список добавленных записей", typeof(List<BaseEntity>))]
    [SwaggerResponse(500, "Произошла ошибка при добавлении записей")]
    public virtual IActionResult AddRange(List<T> models)
    {
        _repository.AddRange(models);
        _repository.SaveChanges();
        if (models.All(p => p.Id > 0))
            return Ok(models);
    
        return StatusCode(500, "Произошла ошибка при добавлении записи");
    }

    /// <summary>
    /// Удаление записи
    /// </summary>
    /// <param name="id">Идентификатор записи</param>
    /// <response code="500">При удалении записи произошла ошибка</response>   
    /// <returns></returns>
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Запись успешно удалена")]
    [SwaggerResponse(404, "Запись не найдена")]
    [SwaggerResponse(500, "Произошла ошибка при удаленнии записи")]
    public virtual IActionResult RemoveByGuid(int id)
    {
        var entity = _repository.Get(id);
        if (entity == null) return NotFound();

        _repository.Remove(id);
        _repository.SaveChanges();
        return Ok();

        return StatusCode(500, "Произошла ошибка при удалении записи");
    }

    /// <summary>
    /// Удаление записей
    /// </summary>
    /// <param name="ids">Идентификаторы записей</param>
    /// <response code="500">При удалении записей произошла ошибка</response>   
    /// <returns></returns>
    [HttpDelete("range")]
    [SwaggerResponse(200, "Записи успешно удалены. В ответе список Guid записей, которые были удалены",
        typeof(List<Guid>))]
    [SwaggerResponse(400, "Не указаны Guid записей, которые необходимо удалить")]
    [SwaggerResponse(500, "Произошла ошибка при удалении записей")]
    public virtual IActionResult RemoveRangeByGuid(List<int> ids)
    {
        if (ids.Count == 0) return BadRequest();
    
        _repository.RemoveRange(ids);
        _repository.SaveChanges();
        return Ok();
    
        return StatusCode(500, "Произошла ошибка при удалении записи");
    }

    /// <summary>
    /// Обновление записи
    /// </summary>
    /// <param name="model">Обновленная запись</param>
    /// <response code="400">Запись не найдена</response>   
    /// <response code="500">При удалении записи произошла ошибка</response>   
    /// <returns></returns>
    [HttpPut]
    [SwaggerResponse(200, "Запись успешно обновлена. Содержит информацию об обновленной записи", typeof(BaseEntity))]
    [SwaggerResponse(404, "Запись не найдена")]
    [SwaggerResponse(500, "При обновлении записи произошла ошибка")]
    public virtual IActionResult Update(T model)
    {
        var fromDb = _repository.Get(model.Id);
        if (fromDb == null) 
            return NotFound("Запись с заданным идентификатором не найдена");

        var res = model.UpdateEntity(_mapper, fromDb);

        try
        {
            _repository.Update(res);
            _repository.SaveChanges();
            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while updating an entity: {e}");
            return StatusCode(500, "Ошибка при обновлении записи");
        }
    }

    /// <summary>
    /// Обновление записей
    /// </summary>
    /// <param name="models">Обновленные записи</param>
    /// <response code="400">Записи не найдены</response>   
    /// <response code="500">При удалении записей произошла ошибка</response>   
    /// <returns></returns>
    [HttpPut("range")]
    [SwaggerResponse(200, "Записи успешно обновлены. Содержит информацию об обновленных записях",
        typeof(List<BaseEntity>))]
    [SwaggerResponse(400, "Нет записей для обновления")]
    [SwaggerResponse(500, "При обновлении записей произошла ошибка")]
    public virtual IActionResult UpdateRange(List<T> models)
    {
        if (models.Count == 0) return BadRequest();
    
        var fromDb = _repository.GetListQuery()
            .Where(p => models.Select(x => x.Id).Contains(p.Id))
            .ToList();
    
        var notFoundGuids = models.Select(p => p.Id)
            .Except(fromDb.Select(p => p.Id))
            .ToList();
    
        if (notFoundGuids.Count != 0) return BadRequest(notFoundGuids);
    
        models = fromDb.Join(models,
                d => d.Id,
                m => m.Id,
                (d, m) => m.UpdateEntity(_mapper, d))
            .ToList();
    
        _repository.UpdateRange(models);
        _repository.SaveChanges();
        return Ok(models);
    }
}