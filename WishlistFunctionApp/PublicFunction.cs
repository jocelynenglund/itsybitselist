using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;
using System;
using System.Threading.Tasks;
using Azure.Core;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.PromiseItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.RevertPromiseItemInWishlist;
using Newtonsoft.Json;
using System.IO;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;

namespace WishlistFunctionApp
{
    public class PublicFunction
    {
        private readonly IMediator _mediator;
        public PublicFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("GetPublicWishlist")]
        public async Task<IActionResult> GetItemById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "public/{id}")]
            HttpRequest req, string id, ILogger log)
        {
            log.LogInformation("GetItemById function requested.");
            try
            {
                var result = await _mediator.Send(new GetWishlistQuery(id));

                return new OkObjectResult(result);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundObjectResult($"Wishlist with id {id} no longer exists");
            }
        }

        public record PatchRequest(string State, Guid? PromiseKey);

        [FunctionName("PromiseItemInWishlist")]
        public async Task<IActionResult> PatchItemById(
                [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "public/{id}/item/{itemId}")]
            HttpRequest req, string id, Guid itemId, ILogger log)
        {
            log.LogInformation("PatchItemById function requested.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PatchRequest request = JsonConvert.DeserializeObject<PatchRequest>(requestBody);

            if (request.State == "Promised")
            {
                var promised = await _mediator.Send(new PromiseItemInWishlistCommand(id, itemId));
                return new OkObjectResult(promised);
            }
            if (request.PromiseKey.HasValue)
            {
                await _mediator.Send(new RevertPromiseItemInWishlistCommand(id, itemId, request.PromiseKey.Value));
                return new OkObjectResult(request.PromiseKey.Value);
            }

            return new NoContentResult();

        }

        [FunctionName("GetItemInPublicWishlist")]
        public async Task<IActionResult> GetItemInWishlist(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "public/{id}/item/{itemId}")]
            HttpRequest req, string id, Guid itemId, ILogger log)
        {
            log.LogInformation("GetItemInWishlist function requested.");
            try
            {
                var item = await _mediator.Send(new GetItemInWishlistQuery(id, itemId));

                return new OkObjectResult(item);
            }
            catch (InvalidOperationException)
            {
                return new NotFoundObjectResult("Item not found");
            }
        }
    }
}
