using ItsyBitseList.App;
using ItsyBitseList.Core.Interfaces.App;
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
    public class PublicFunction
    {
        private readonly IWishlistApp _application;
        public PublicFunction(IWishlistApp application)
        {
            _application = application;
        }

        [FunctionName("GetPublicWishlist")]
        public async Task<IActionResult> GetItemById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "public/{id}")]
            HttpRequest req, string id, ILogger log)
        {
            log.LogInformation("GetItemById function requested.");
            var response = await _application.GetWishlist(id.ToString());
            return response.AsActionResult();
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
                var response = await _application.PromiseItem(id, itemId);
                return response.AsActionResult();
            }
            if (request.PromiseKey.HasValue)
            {
                var response = await _application.RevertPromise(id, itemId, request.PromiseKey.Value);
                return response.AsActionResult();
            }

            return new NoContentResult();

        }

        [FunctionName("GetItemInPublicWishlist")]
        public async Task<IActionResult> GetItemInWishlist(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "public/{id}/item/{itemId}")]
            HttpRequest req, string id, Guid itemId, ILogger log)
        {
            log.LogInformation("GetItemInWishlist function requested.");
            var response = await _application.GetItemInWishlist(id, itemId);

            return response.AsActionResult();
        }
    }
}
