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
            return await PagedList<Owner>.ToPagedList(FindAll(), ownerParameters.PageNumber, ownerParameters.PageSize);
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
