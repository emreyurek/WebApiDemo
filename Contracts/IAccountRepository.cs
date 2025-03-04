using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        IEnumerable<Account> AccountsByOwner(Guid ownerId);
        Task<PagedList<ShapedEntity>> GetAllAccountsAsync(AccountParameters accountParameters);
        Task<Account> GetAccountByIdAsync(Guid accountId);
        Task<Account> GetAccountWithDetailsAsync(Guid accountId);
        void CreateAccount(Account account);
        void UpdateAccount(Account account);
        void DeleteAccount(Account account);
    }
}
