using ItsyBitseList.App;
using ItsyBitseList.Core.Interfaces.App;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
namespace WishlistFunctionApp
{
    public class WishlistFunction
    {
        private readonly IMediator _mediator;
        private readonly IWishlistApp _application;
        private readonly HttpContext _httpContext; 
        public WishlistFunction(IWishlistApp application,HttpContextAccessor httpContextAccessor)
        {
            _application = application;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [FunctionName("GetWishlist")]
        public async Task<IActionResult> GetWishlist(
                       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wishlist/{id}")] HttpRequest req, string id, ILogger log)
        {
            log.LogInformation("GetWishlist function requested.");
            var response = await _application.GetWishlist(id.ToString());
            return response.AsActionResult();
        }

        [FunctionName("CreateWishlist")]
        public async Task<IActionResult> CreateWishlist([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wishlist")] HttpRequest req, ILogger log)
        {
            log.LogInformation("CreateWishlist function requested.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<WishlistCreationRequest>(requestBody);
            var response = await _application.CreateWishlist(request);

            _httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");
            return new CreatedResult($"/wishlist/{response.Result}", response.Result);
        }

        [FunctionName("UpdateWishlist")]
        public async Task<IActionResult> Patch([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "wishlist/{id}")] HttpRequest req, Guid id, ILogger log)
        {
            log.LogInformation("UpdateWishlist function requested.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<WishlistUpdateRequest>(requestBody);
            var response = await _application.UpdateWishlist(id, request);

            _httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");

            return response.AsActionResult();
        }
        [FunctionName("AddItemToWishlist")]
        public async Task<IActionResult> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "wishlist/{id}/item")] HttpRequest req, Guid id, ILogger log)
        {
            log.LogInformation("AddItemToWishlist function requested.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<ItemCreationRequest>(requestBody);
            var response = await _application.AddItemToWishlist(id, request);

            _httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");
            return new CreatedResult($"/wishlist/{id}/item/{response.Result}", response.Result);
        }

        [FunctionName("GetItemInWishlist")] 
        public async Task<IActionResult> GetItemInWishlist([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wishlist/{id}/item/{itemId}")] HttpRequest req, Guid id, Guid itemId, ILogger log)
        {
            log.LogInformation("GetItemInWishlist function requested.");
            var response = await _application.GetItemInWishlist(id.ToString(), itemId);


            return response.AsActionResult();
        }

        [FunctionName("DeleteItemInWishlist")]
        public async Task<IActionResult> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "wishlist/{id}/item/{itemId}")] HttpRequest req, Guid id, Guid itemId, ILogger log)
        {
            log.LogInformation("DeleteItemInWishlist function requested.");
            var response = await _application.DeleteWishlistItem(id, itemId);

            return response.AsActionResult();
        }
    }
}
