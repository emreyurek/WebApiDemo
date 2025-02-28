
namespace Entities.RequestFeatures
{
    public class OwnerParameters : QueryStringParameters
    {
        public OwnerParameters()
        {
            OrderBy = "name"; // default
        }
        public uint MinYearOfBirth { get; set; } // default value is 0
        public uint MaxYearOfBirth { get; set; } = (uint)DateTime.Now.Year;
        public bool ValidYearRange => MaxYearOfBirth > MinYearOfBirth;
        public string? Name { get; set; }
    }
}
