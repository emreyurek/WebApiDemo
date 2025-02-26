using AccountOwnerServer.Filters;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OwnerController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners([FromQuery] OwnerParameters ownerParameters)
        {
            if (!ownerParameters.ValidYearRange)
            {
                return BadRequest("Max year of birth cannot be less than min year of birth");
            }

            var owners = await _repository.Owner.GetAllOwnersAsync(ownerParameters);

            var metadata = new
            {
                owners.TotalCount,
                owners.PageSize,
                owners.CurrentPage,
                owners.TotalPages,
                owners.HasNext,
                owners.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var ownerResults = _mapper.Map<IEnumerable<OwnerDto>>(owners);
            return Ok(ownerResults);
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public async Task<IActionResult> GetOwnerById([FromRoute] Guid id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(id);
            if (owner is null)
            {
                throw new EntityNotFoundException($"Owner with id: {id}, hasn't been found in db.");
            }

            var ownerResult = _mapper.Map<OwnerDto>(owner);
            return Ok(ownerResult);
        }

        [HttpGet("{id}/account")]
        public async Task<IActionResult> GetOwnerWithDetails(Guid id)
        {
            var owner = await _repository.Owner.GetOwnerWithDetailsAsync(id);
            if (owner is null)
            {
                throw new EntityNotFoundException($"Owner with id: {id}, hasn't been found in db.");
            }

            var ownerResult = _mapper.Map<OwnerDto>(owner); //nested mapping 
            return Ok(ownerResult);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerForCreationDto owner)
        {
            var ownerEntity = _mapper.Map<Owner>(owner);

            _repository.Owner.CreateOwner(ownerEntity);
            await _repository.SaveAsync();

            var createOwner = _mapper.Map<OwnerDto>(ownerEntity);
            return CreatedAtRoute("OwnerById", new { id = createOwner.Id }, createOwner); //201
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateOwner([FromRoute] Guid id, [FromBody] OwnerForUpdateDto owner)
        {
            var ownerEntity = await _repository.Owner.GetOwnerByIdAsync(id);
            if (ownerEntity is null)
            {
                throw new EntityNotFoundException($"Owner with id: {id}, hasn't been found in db.");
            }

            _mapper.Map(owner, ownerEntity); //ownerEntity updated

            _repository.Owner.UpdateOwner(ownerEntity);
            await _repository.SaveAsync();

            return NoContent(); //204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner([FromRoute] Guid id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(id);
            if (owner is null)
            {
                throw new EntityNotFoundException($"Owner with id: {id}, hasn't been found in db.");
            }
            if (_repository.Account.AccountsByOwner(id).Any())
            {
                throw new ConflictException("Cannot delete owner. It has related accounts. Delete those accounts first");
            }

            _repository.Owner.DeleteOwner(owner);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
