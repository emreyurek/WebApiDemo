using System.Net;

namespace Entities.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, (int)HttpStatusCode.BadRequest)
        {

        }
    }
}
