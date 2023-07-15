using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.AddItemToWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.DeleteItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetItemInWishlist.GetItemInWishlist;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Queries.GetWishlist;

namespace WishlistFunctionApp
{
    public class WishlistFunction
    {
        private readonly IMediator _mediator;
        private readonly HttpContext _httpContext; 
        public WishlistFunction(IMediator mediator,HttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [FunctionName("GetWishlist")]
        public async Task<IActionResult> GetWishlist(
                       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wishlist/{id}")] HttpRequest req, string id, ILogger log)
        {
            log.LogInformation("GetWishlist function requested.");
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
        public record WishlistCreationRequest(string Name);
        [FunctionName("CreateWishlist")]
        public async Task<IActionResult> CreateWishlist([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wishlist")] HttpRequest req, ILogger log)
        {
            log.LogInformation("CreateWishlist function requested.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<WishlistCreationRequest>(requestBody);
            var result = await _mediator.Send(new CreateWishlistCommand(null, request.Name ));
            _httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");
            return new CreatedResult($"/wishlist/{result}", result);
        }


        public record ItemCreationRequest(string Details, Uri? Link);
        [FunctionName("AddItemToWishlist")]
        public async Task<IActionResult> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wishlist/{id}/item")] HttpRequest req, Guid id, ILogger log)
        {
            log.LogInformation("AddItemToWishlist function requested.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<ItemCreationRequest>(requestBody);
            var result = await _mediator.Send(new AddItemToWishlistCommand(id, request.Details, request.Link));
            _httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");
            return new CreatedResult($"/wishlist/{id}/item/{result}", result);
        }

        [FunctionName("GetItemInWishlist")] 
        public async Task<IActionResult> GetItemInWishlist([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wishlist/{id}/item/{itemId}")] HttpRequest req, Guid id, Guid itemId, ILogger log)
        {
            log.LogInformation("GetItemInWishlist function requested.");
            var result = await _mediator.Send(new GetItemInWishlistQuery(id.ToString(), itemId));

            return new OkObjectResult(result);
        }

        [FunctionName("DeleteItemInWishlist")]
        public async Task<IActionResult> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "wishlist/{id}/item/{itemId}")] HttpRequest req, Guid id, Guid itemId, ILogger log)
        {
            log.LogInformation("DeleteItemInWishlist function requested.");
            await _mediator.Send(new DeleteItemInWishlistCommand(id, itemId));
            
            return new NoContentResult();
        }
    }
}
