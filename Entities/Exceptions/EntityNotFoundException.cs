using System.Net;

namespace Entities.Exceptions
{
    public class EntityNotFoundException : BaseException
    {
        public EntityNotFoundException(string message) : base(message, (int)HttpStatusCode.NotFound)
        {

        }
    }
}
