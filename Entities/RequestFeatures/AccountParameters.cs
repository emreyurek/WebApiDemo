
namespace Entities.RequestFeatures
{
    public class AccountParameters : QueryStringParameters
    {
        public uint MinDateCreated { get; set; } // default value is 0
        public uint MaxDateCreated { get; set; } = (uint)DateTime.Now.Year;
        public bool ValidYearRange => MaxDateCreated > MinDateCreated;
    }
}
