using System.Dynamic;
using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        private ISortHelper<Account> _sortHelper;
        private IDataShaper<Account> _dataShaper;
        public AccountRepository(RepositoryContext repositoryContext, ISortHelper<Account> sortHelper, IDataShaper<Account> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public IEnumerable<Account> AccountsByOwner(Guid ownerId)
        {
            return FindByCondition(a => a.OwnerId.Equals(ownerId)).ToList();
        }

        public void CreateAccount(Account account)
        {
            Create(account);
        }

        public void DeleteAccount(Account account)
        {
            Delete(account);
        }

        public async Task<Account> GetAccountByIdAsync(Guid accountId)
        {
            return await FindByCondition(a => a.Id.Equals(accountId))
                   .FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccountWithDetailsAsync(Guid accountId)
        {
            return await FindByCondition(a => a.Id.Equals(accountId))
                    .Include(o => o.Owner)
                    .FirstOrDefaultAsync();
        }

        public async Task<PagedList<ExpandoObject>> GetAllAccountsAsync(AccountParameters accountParameters)
        {
            var accounts = FindByCondition(account => (account.DateCreated.Year >= accountParameters.MinDateCreated) &&
                                                      (account.DateCreated.Year <= accountParameters.MaxDateCreated));

            SearchByAccountType(ref accounts, accountParameters.AccountType);

            var sortedAccounts = _sortHelper.ApplySort(accounts, accountParameters.OrderBy);

            var shapedAccounts = _dataShaper.ShapeData(sortedAccounts, accountParameters.Fields);

            return await PagedList<ExpandoObject>.ToPagedList(shapedAccounts, accountParameters.PageNumber, accountParameters.PageSize);
        }

        private void SearchByAccountType(ref IQueryable<Account> accounts, string accountType)
        {
            if (!accounts.Any() || string.IsNullOrWhiteSpace(accountType))
                return;

            accounts = accounts.Where(ac => ac.AccountType.ToLower().Contains(accountType.Trim().ToLower()));
        }

        public void UpdateAccount(Account account)
        {
            Update(account);
        }
    
    }
}
