using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerServer.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public AccountController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _repository.Account.GetAllAccounts();
                _logger.LogInfo("Returned all accounts from database.");

                var accountsResults = _mapper.Map<IEnumerable<AccountDto>>(accounts);
                return Ok(accountsResults);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllAccounts action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "AccountById")]
        public IActionResult GetOneAccount([FromRoute] Guid id)
        {
            try
            {
                var account = _repository.Account.GetAccountById(id);
                if (account is null)
                {
                    _logger.LogError($"Account with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned account with id:{id}");

                var accountResult = _mapper.Map<AccountDto>(account);
                return Ok(accountResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOneAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/owner")]
        public IActionResult GetAccountWithDetails([FromRoute] Guid id)
        {
            try
            {
                var account = _repository.Account.GetAccountWithDetails(id);
                if (account is null)
                {
                    _logger.LogError($"Account with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned account with details for id: {id}");

                    var accountResult = _mapper.Map<AccountForDetailsDto>(account);
                    return Ok(accountResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] AccountForCreationDto account)
        {
            try
            {
                if (account is null)
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null"); //400
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid account object sent from client");
                    return BadRequest("Invalid model object"); //400
                }

                var accountEntity = _mapper.Map<Account>(account);

                _repository.Account.CreateAccount(accountEntity);
                _repository.Save();

                var createAccount = _mapper.Map<AccountDto>(accountEntity);
                return CreatedAtRoute("AccountById", new { id = createAccount.Id }, createAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccount([FromRoute] Guid id, [FromBody] AccountForUpdateDto account)
        {
            try
            {
                if (account is null)
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null"); //400
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid account object sent from client");
                    return BadRequest("Invalid model object"); //400
                }

                var accountEntity = _repository.Account.GetAccountById(id);
                if (accountEntity is null)
                {
                    _logger.LogError($"Account with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(account, accountEntity);

                _repository.Account.UpdateAccount(accountEntity);
                _repository.Save();

                return NoContent();//204
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount([FromRoute] Guid id)
        {
            try
            {
                var account = _repository.Account.GetAccountById(id);
                if (account is null)
                {
                    _logger.LogError($"Account with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Account.DeleteAccount(account);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAccount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
