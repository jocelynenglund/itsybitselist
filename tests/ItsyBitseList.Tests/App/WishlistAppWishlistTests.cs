using FluentAssertions;
using ItsyBitseList.App;
using ItsyBitseList.Core.Constants;
using ItsyBitseList.Core.Interfaces.App;
using ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands;
using ItsyBitseList.Core.WishlistCollectionAggregate;
using MediatR;
using Moq;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.Tests.App
{
    public class WishlistAppWishlistTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IWishlistApp _sut;
        private const string ID1 = "59fdc665-e3c3-4711-ba59-5de3f7071559";
        private const string ID2 = "1a76231a-3952-4d69-9361-12f3088f4221";


        public WishlistAppWishlistTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new WishlistApp(_mediatorMock.Object);
        }
        [Fact]
        public async Task ApplicationCreateWishlist_ShouldReturnResultObjectType()
        {
            // Arrange
            var request = GetWishlistCreationRequest("WishlistName");
            // Act
            var result = await _sut.CreateWishlist(request);

            // Assert
            result.Should().BeOfType(typeof(Response<Guid>));
        }

        [Theory]
        [InlineData("WishlistName", ID1)]
        [InlineData("WishlistName", ID2)]
        public async Task ApplicationShouldReturnResultWithId(string name, Guid ID)
        {
            // Arrange
            var request = GetWishlistCreationRequest(name);
            MockMediator<Guid>(ID);
            
            // Act
            var response = await _sut.CreateWishlist(request);

            // Assert
            response.Should().BeOfType(typeof(Response<Guid>));
            response.Result.Should().Be(ID);
        }

        [Fact]
        public async Task ApplicationGetWishlistThrowingInvalidOperation_ShouldReturnResultWithStatusNotFound()
        {
            MockMediatorException<WishListDetails>(new InvalidOperationException(ErrorMessages.ItemNotFound));

            var response = await _sut.GetWishlist(ID1);
            response.Result.Should().Be(null);
            response.Status.Should().Be(Status.NotFound);
            response.ErrorMessage.Should().Be(ErrorMessages.ItemNotFound);
        }


        [Fact]
        public async Task ApplicationGetWishlistThrowingException_ShouldReturnResultWithStatusError()
        {
            MockMediatorException<WishListDetails>(new Exception(ErrorMessages.UnexpectedError));

            var response = await _sut.GetWishlist(ID1);
            response.Result.Should().Be(null);
            response.Status.Should().Be(Status.Error);
            response.ErrorMessage.Should().Be(ErrorMessages.UnexpectedError);
        }

        [Fact]
        public async Task ApplicationDeleteWishlist_ShouldReturnResultWithSuccessCode()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteWishlistCommand>(), It.IsAny<CancellationToken>()));

            var response = await _sut.DeleteWishlist(Guid.Parse(ID1));
            response.Result.Should().BeOfType(typeof(Unit));
            response.Status.Should().Be(Status.Success);
        }

        [Fact]
        public async Task ApplicationDeleteWishlistWithWrongId_ShouldReturnNotFoundStatusCode()
        {
            MockMediatorException<Unit>(new InvalidOperationException(ErrorMessages.WishlistNotFound));
            
            var response = await _sut.DeleteWishlist(Guid.Parse(ID1));

            response.Result.Should().Be(default);
            response.Status.Should().Be(Status.NotFound);
        }

        [Fact]
        public async Task ApplicationDeleteWishlistWithUnexpectedException_ShouldReturnErrorCode()
        {
            MockMediatorException<Unit>(new Exception(ErrorMessages.UnexpectedError));

            var response = await _sut.DeleteWishlist(Guid.Parse(ID1));
            response.Result.Should().Be(default);
            response.Status.Should().Be(Status.Error);

        }

        [Fact]
        public async Task ApplicationAddItemToWishlist_ShouldReturnItemId()
        {
            MockMediator<Guid>(Guid.Parse(ID1));

            var request = GetItemCreationRequest("item", new Uri("http://www.google.com"));

            var response = await _sut.AddItemToWishlist(Guid.Parse(ID1), request);

            response.Should().BeOfType(typeof(Response<Guid>));
            response.Result.Should().Be(Guid.Parse(ID1));
        }

        [Fact]
        public async Task ApplicationAddItemToWishlist_ShouldReturnErrorCodeOnException()
        {
            MockMediatorException<Guid>(new Exception(ErrorMessages.UnexpectedError));

            var request = GetItemCreationRequest("item", new Uri("http://www.google.com"));

            var response = await _sut.AddItemToWishlist(Guid.Parse(ID1), request);
            response.Status.Should().Be(Status.Error);
        }

        [Fact]
        public async Task ApplicationGetItemInWishlist_ShouldReturnItem()
        {
            var item = new ItemDetails(Guid.Parse(ID1), State.Promised, "item");
            MockMediator<ItemDetails>(item);

            var response = await _sut.GetItemInWishlist(ID1, Guid.Parse(ID2));

            response.Should().BeOfType(typeof(Response<ItemDetails>));
            response.Result.Should().Be(item);
        }

        [Fact]
        public async Task ApplicationGetItemInWishlist_ShouldReturnErrorCodeOnException()
        {
            var item = new ItemDetails(Guid.Parse(ID1), State.Promised, "item");
            MockMediatorException<ItemDetails>(new Exception(ErrorMessages.UnexpectedError));

            var response = await _sut.GetItemInWishlist(ID1, Guid.Parse(ID2));

            response.Status.Should().Be(Status.Error);
        }

        [Fact]
        public async Task ApplicationDeleteWishlistItem_ShouldReturnSuccessCode()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteItemInWishlistCommand>(), It.IsAny<CancellationToken>()));

            var response = await _sut.DeleteWishlistItem(Guid.Parse(ID1), Guid.Parse(ID2));

            response.Status.Should().Be(Status.Success);
        }

        [Fact]
        public async Task ApplicationDeleteWishlistItem_ShouldReturnErrorCodeOnException()
        {
            MockMediatorException<Unit>(new Exception(ErrorMessages.UnexpectedError));

            var response = await _sut.DeleteWishlistItem(Guid.Parse(ID1), Guid.Parse(ID2));

            response.Status.Should().Be(Status.Error);
        }

        [Fact]
        public async Task ApplicationPromiseItem_ShouldReturnPromiseKey()
        {
            MockMediator<Guid>(Guid.Parse(ID1));

            var response = await _sut.PromiseItem(ID1, Guid.Parse(ID2));

            response.Should().BeOfType(typeof(Response<Guid>));
            response.Result.Should().Be(Guid.Parse(ID1));
        }

        [Fact]
        public async Task ApplicationRevertPromise_ShouldReturnPromiseKey()
        {
            MockMediator<Guid>(Guid.Parse(ID2));

            var response = await _sut.RevertPromise(wishlistId:ID1, itemId:Guid.Parse(ID2), promiseKey:Guid.Parse(ID2));

            response.Should().BeOfType(typeof(Response<Guid>));
            response.Result.Should().Be(Guid.Parse(ID2));
        }

        private ItemCreationRequest GetItemCreationRequest(string name, Uri url)
        {
            return new ItemCreationRequest(name, url);
        }

        private WishlistCreationRequest GetWishlistCreationRequest(string Name, string owner = "Anonymous")
        {
            return new WishlistCreationRequest(owner, Name);
        }

        private void MockMediator<T>(T response)
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<T>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
        }

        private void MockMediatorException<T>(Exception exception) 
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<T>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
        }
    }
}
