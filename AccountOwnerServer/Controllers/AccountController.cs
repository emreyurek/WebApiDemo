using AccountOwnerServer.Filters;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccountOwnerServer.Controllers
{
    [Route("api/account")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        private LinkGenerator _linkGenerator;

        public AccountController(IRepositoryWrapper repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts([FromQuery] AccountParameters accountParameters)
        {

            if (!accountParameters.ValidYearRange)
            {
                return BadRequest("Max year of date created cannot be less than min year of date created");
            }

            var accounts = await _repository.Account.GetAllAccountsAsync(accountParameters);

            var metadata = new
            {
                accounts.TotalCount,
                accounts.PageSize,
                accounts.CurrentPage,
                accounts.TotalPages,
                accounts.HasNext,
                accounts.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var shapedAccounts = accounts.Select(o => o.Entity).ToList();
            for (var index = 0; index < accounts.Count(); index++)
            {
                var accountLinks = CreateLinksForOwner(accounts[index].Id, accountParameters.Fields);
                shapedAccounts[index].Add("Links", accountLinks);
            }

            var ownersWrapper = new LinkCollectionWrapper<Entity>(shapedAccounts);
            return Ok(CreateLinksForOwners(ownersWrapper));
        }

        [HttpGet("{id}", Name = "AccountById")]
        public async Task<IActionResult> GetAccountById([FromRoute] Guid id)
        {
            var account = await _repository.Account.GetAccountByIdAsync(id);

            var accountResult = _mapper.Map<AccountDto>(account);
            return Ok(accountResult);
        }

        [HttpGet("{id}/owner")]
        public async Task<IActionResult> GetAccountWithDetails([FromRoute] Guid id)
        {
            var account = await _repository.Account.GetAccountWithDetailsAsync(id);
            if (account is null)
            {
                throw new EntityNotFoundException($"Account with id: {id}, hasn't been found in db.");
            }

            var accountResult = _mapper.Map<AccountForDetailsDto>(account);
            return Ok(accountResult);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateAccount([FromBody] AccountForCreationDto account)
        {
            var accountEntity = _mapper.Map<Account>(account);

            _repository.Account.CreateAccount(accountEntity);
            await _repository.SaveAsync();

            var createAccount = _mapper.Map<AccountDto>(accountEntity);
            return CreatedAtRoute("AccountById", new { id = createAccount.Id }, createAccount);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid id, [FromBody] AccountForUpdateDto account)
        {
            var accountEntity = await _repository.Account.GetAccountByIdAsync(id);
            if (accountEntity is null)
            {
                throw new EntityNotFoundException($"Account with id: {id}, hasn't been found in db.");
            }

            _mapper.Map(account, accountEntity);

            _repository.Account.UpdateAccount(accountEntity);
            await _repository.SaveAsync();

            return NoContent();//204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            var account = await _repository.Account.GetAccountByIdAsync(id);
            if (account is null)
            {
                throw new EntityNotFoundException($"Account with id: {id}, hasn't been found in db.");
            }

            _repository.Account.DeleteAccount(account);
            await _repository.SaveAsync();

            return NoContent();
        }

        private IEnumerable<Link> CreateLinksForOwner(Guid id, string fields = "")
        {
            var links = new List<Link>
                        {
                            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetAccountById), values: new { id, fields }), "self", "GET"),

                            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteAccount), values: new { id }), "delete_account", "DELETE"),

                            new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateAccount), values: new { id }), "update_account", "PUT")
                        };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForOwners(LinkCollectionWrapper<Entity> accountsWrapper)
        {
            accountsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetAllAccounts), values: new { }), "self", "GET"));

            return accountsWrapper;
        }
    }
}
