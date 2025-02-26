using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
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
                                         (owner.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth))
                                         .OrderBy(owner => owner.Name);

            return await PagedList<Owner>.ToPagedList(owners, ownerParameters.PageNumber, ownerParameters.PageSize);
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
