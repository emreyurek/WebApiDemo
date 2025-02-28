using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        private ISortHelper<Owner> _sortHelper;
        public OwnerRepository(RepositoryContext repositoryContext, ISortHelper<Owner> sortHelper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }

        public async Task<PagedList<Owner>> GetAllOwnersAsync(OwnerParameters ownerParameters)
        {
            var owners = FindByCondition(owner => (owner.DateOfBirth.Year >= ownerParameters.MinYearOfBirth) &&
                                         (owner.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth));

            SearchByName(ref owners, ownerParameters.Name);

            var sortedOwners = _sortHelper.ApplySort(owners, ownerParameters.OrderBy);

            return await PagedList<Owner>.ToPagedList(sortedOwners, ownerParameters.PageNumber, ownerParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Owner> owners, string ownerName)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(ownerName))
                return;

            owners = owners.Where(ow => ow.Name.ToLower().Contains(ownerName.Trim().ToLower()));
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid ownerId)
        {
            return await FindByCondition(owner => owner.Id.Equals(ownerId))
                                         .FirstOrDefaultAsync();
        }

        public async Task<Owner> GetOwnerWithDetailsAsync(Guid ownerId)
        {
            return await FindByCondition(owner => owner.Id.Equals(ownerId))
                                        .Include(ac => ac.Accounts)
                                         .FirstOrDefaultAsync();
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }

    }
}
