using ItsyBitseList.Core.Constants;
using ItsyBitseList.Core.WishlistCollectionAggregate;

namespace ItsyBitseList.Core.Interfaces.App
{
    public record Response<T>
    {
        public T? Result { get; init; }
        public Status Status { get; set; }
        public string ErrorMessage { get; internal set; }

        public Response(T? result)
        {
            Result = result;
            Status = Status.Success;
        }
        public Response(Status status,
            string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }
    }
    public record NotFoundErrorResponse<T> : Response<T>
    {
        public NotFoundErrorResponse() : base(Status.NotFound,ErrorMessages.ItemNotFound) { }
    }
    public record UnexpectedErrorResponse<T> : Response<T>
    {
        public UnexpectedErrorResponse() : base(Status.Error, ErrorMessages.UnexpectedError) { }
    }

    public enum Status
    {
        None,
        Success,
        NotFound,
        Error
    }
}
