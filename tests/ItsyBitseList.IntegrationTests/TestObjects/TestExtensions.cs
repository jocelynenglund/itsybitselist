using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItsyBitseList.IntegrationTests.TestObjects
{
    public static class TestExtensions
    {
        public static async Task<T> Parse<T>(this HttpResponseMessage wishlistResponse)
        {

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter(allowIntegerValues: true) },
                PropertyNameCaseInsensitive = true
            };
            return await wishlistResponse.Content.ReadFromJsonAsync<T>(options);
        }
    }
}
