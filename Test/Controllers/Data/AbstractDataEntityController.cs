using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Test.DTO;
using Test.Filters;
using Test.Models;
using Test.Services;

namespace Test.Controllers.Data
{
    /// <summary>
    /// Base controller with basic CRUD operations for entities in the database.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity handled by this controller.</typeparam>
    /// <typeparam name="TDto">Type of DTO used as input and output from this controller.</typeparam>
    /// <typeparam name="TId">Type of the ID property both in <typeparamref name="TEntity"/> and <typeparamref name="TDto"/>.</typeparam>
    public abstract class AbstractDataEntityController<TEntity, TDto, TId> : ControllerBase
        where TEntity : IEntity<TId>
        where TDto : IDto<TId>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AbstractDataEntityController<,,>));

        /// <summary>
        /// Service to handle entities from this controller.
        /// </summary>
        protected readonly IStoredDataService<TEntity, TDto, TId> Service;

        /// <summary>
        /// Creates a new entity of this controller.
        /// </summary>
        /// <param name="service"></param>
        public AbstractDataEntityController(IStoredDataService<TEntity, TDto, TId> service) => this.Service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// Retrieves all entities from this collection with an optional <paramref name="search"/> filter and paging arguments.
        /// </summary>
        /// <param name="search">Optional search string to filter the results. The terms in this string are searched in all the entity properties.</param>
        /// <param name="limit">Max number of entities to return.</param>
        /// <param name="offset">Number of entities to bypass from the collection.</param>
        /// <param name="orderBy">Name of the column to sort the results.</param>
        /// <param name="orderAsc">Indicates wether the results should be returned in ascending (<c>true</c>) or descending (<c>false</c>) order.</param>
        /// <returns cref="PagedResult{TDto}"><see cref="PagedResult{TDto}"/> containing the results.</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("")]
        public virtual IActionResult GetAll(string search = null, int limit = 0, int offset = 0, string orderBy = null, bool orderAsc = true)
        {
            logger.Debug($"GET {typeof(TEntity).Name}");
            var result = Service.RetrieveAll(null, search, offset, limit, orderBy, orderAsc);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the entity from this collection identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the entity to retrieve.</param>
        /// <returns>DTO containig information about the entity from this collection specified by <paramref name="id"/>, or a "404 Not Found" status if the entity doesn't exist.</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("{id}")]
        public virtual IActionResult Get(TId id)
        {
            logger.Debug($"GET {typeof(TEntity).Name} with id {id}");
            var result = Service.Retrieve(id);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        /// <summary>
        /// Creates a new entity in this collection.
        /// </summary>
        /// <param name="dto">Data to create the new entity.</param>
        /// <returns>When successful, returns the data of the newly created entity; when data in the post body is incorrect or incomplete, returns a "400 Bad Request" status
        /// indicating any validation errors found.</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("")]
        public virtual IActionResult Add([Microsoft.AspNetCore.Mvc.FromBody]TDto dto)
        {
            try
            {
                var result = Service.Create(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.Error($"An error ocurred when trying to add an entity of type {typeof(TEntity).Name}", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates the entity specified by <paramref name="id"/> in this collection with the data provided in the body.
        /// </summary>
        /// <param name="id">ID of the entity to update.</param>
        /// <param name="dto">Data to replace in the entity.</param>
        /// <returns>When successful, returns the data of the updated entity; when data in the post body is incorrect or incomplete, returns a "400 Bad Request" status
        /// indicating any validation errors found; when the entity identified by <paramref name="id"/> doesn't exist, returns a "404 Not Found" status.</returns>
        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("{id}")]
        public virtual IActionResult Update(TId id, [Microsoft.AspNetCore.Mvc.FromBody]TDto dto)
        {
            if (dto == null) return BadRequest("No data was provided.");
            dto.Id = id;
            try
            {
                var result = Service.Update(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.Error($"An error ocurred when trying to update an entity of type {typeof(TEntity).Name}", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes the entity specified by <paramref name="id"/> in this collection.
        /// </summary>
        /// <param name="id">ID of the entity to delete.</param>
        /// <returns>When successful, returns a "200 Success" status, otherwise, returns a "500 Internal Server Error" when something went wrong.</returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("{id}")]
        public virtual IActionResult Delete(TId id)
        {
            if (id.Equals(default(TId))) return BadRequest();

            try { if (Service.Delete(id)) return Ok(); }
            catch (KeyNotFoundException) { return NotFound(); }
            return NotFound();
        }

        /// <summary>
        /// Executes a search on the entity handled by this controller using the provided <paramref name="filters"/> and returns the result as a <see cref="PagedResult{TDto}"/>.
        /// </summary>
        /// <param name="filters">Search and filtering information.</param>
        /// <returns><see cref="PagedResult{TDto}"/> containing the results of the search.</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("search")]
        public virtual IActionResult Search(PagedSearchAndFilterInfo filters)
        {
            DTO.PagedResult<TDto> result = null;
            try
            {
                result = Service.RetrieveAll(filters);
                if (result == null) throw new Exception($"Unexpected error.");  // This should never happen.
            }
            catch (Exception ex)
            {
                logger.Error($"Error searching entities of type {typeof(TEntity)}", ex);
                return BadRequest();
            }
            return Ok(result);
        }

        /// <summary>
        /// Executes a search using the provided <paramref name="filters"/> and returns the result as an array of <see cref="int"/>.
        /// </summary>
        /// <param name="filters">Search and filtering information.</param>
        /// <returns>Array of <see cref="int"/> containing the results of the search.</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("getallids")]
        public virtual IActionResult GetAllIds(PagedSearchAndFilterInfo filters)
        {
            logger.Debug($"GetAllIds {typeof(TEntity).Name}");
            try
            {
                var result = Service.GetAllIds(filters);
                if (result == null)
                    throw new Exception($"Unexpected error.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error($"Error getting all the IDs for {typeof(TEntity).Name}.", ex);
                return NotFound();
            }
        }
    }
}
