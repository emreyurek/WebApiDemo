using System.Net;

namespace Entities.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict)
        {

        }

    }
}
