using AccountOwnerServer.Filters;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerServer.Controllers
{
    [Route("api/account")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public AccountController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _repository.Account.GetAllAccountsAsync();

            var accountsResults = _mapper.Map<IEnumerable<AccountDto>>(accounts);
            return Ok(accountsResults);
        }

        [HttpGet("{id}", Name = "AccountById")]
        public async Task<IActionResult> GetOneAccount([FromRoute] Guid id)
        {
            var account = await _repository.Account.GetAccountByIdAsync(id);
            if (account is null)
            {
                throw new EntityNotFoundException($"Account with id: {id}, hasn't been found in db.");
            }

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

    }
}
