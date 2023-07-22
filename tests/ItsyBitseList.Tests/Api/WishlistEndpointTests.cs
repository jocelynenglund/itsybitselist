using AutoFixture;
using FluentAssertions;
using ItsyBitseList.Api.Controllers;
using ItsyBitseList.Core.Constants;
using ItsyBitseList.Core.Interfaces.App;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace ItsyBitseList.Tests.Api
{
    public class WishlistEndpointTests
    {
        private Mock<IWishlistApp> _applicationMock;
        private WishlistController _sut;
        public WishlistEndpointTests()
        {
            _applicationMock = new Mock<IWishlistApp>();
            _sut = new WishlistController(_applicationMock.Object);
        }

        [Fact]
        public async Task GetWishlistWithValidID_ReturnsOkResult()
        {
            var mockResult = new Fixture().Create<WishListDetails>();
            _applicationMock.Setup(w => w.GetWishlist(It.IsAny<string>())).ReturnsAsync(new Response<WishListDetails>(mockResult));

            var response = await _sut.GetWishlist(default, Guid.Parse("59fdc665-e3c3-4711-ba59-5de3f7071559"));

            response.Should().BeOfType(typeof(OkObjectResult));
        }
        [Fact]
        public async Task GetWishlistWithInvalidID_ReturnsNotFoundResult()
        {
            _applicationMock.Setup(w=>w.GetWishlist(It.IsAny<string>())).ReturnsAsync(new Response<WishListDetails>(Status.NotFound, ErrorMessages.WishlistNotFound));

            var response = await _sut.GetWishlist(default, Guid.Parse("59fdc665-e3c3-4711-ba59-5de3f7071559"));

            response.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        [Fact]
        public async Task GetWishlistWithException_ReturnsInternalServerException() { 
            _applicationMock.Setup(w=>w.GetWishlist(It.IsAny<string>())).ReturnsAsync(new Response<WishListDetails>(Status.Error, ErrorMessages.UnexpectedError));

            var response = (StatusCodeResult) await _sut.GetWishlist(default, Guid.Parse("59fdc665-e3c3-4711-ba59-5de3f7071559"));

            response.Should().BeOfType(typeof(StatusCodeResult));
            response.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DeleteWishlistWithValidId_ReturnsNoContentResult()
        {
            _applicationMock.Setup(w=>w.DeleteWishlist(It.IsAny<Guid>(), default)).ReturnsAsync(new Response<Unit>(Unit.Value));

            var response = await _sut.DeleteWishlist(default, Guid.Parse("59fdc665-e3c3-4711-ba59-5de3f7071559"));

            response.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public async Task DeleteWishlistWithInvalidID_ReturnsNotFoundResult()
        {
            _applicationMock.Setup(w => w.DeleteWishlist(It.IsAny<Guid>(), default)).ReturnsAsync(new Response<Unit>(Status.NotFound, ErrorMessages.WishlistNotFound));

            var response = await _sut.DeleteWishlist(default, Guid.Parse("59fdc665-e3c3-4711-ba59-5de3f7071559"));

            response.Should().BeOfType(typeof(NotFoundObjectResult));
        }

    }
}
