using ChatBotTakeAPI.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotTakeAPI.Service
{
    class HTTPRequestService
    {
        private readonly string USER_AGENT = "Mozilla/5.0";

        private static readonly HTTPRequestService instance = new HTTPRequestService();
        private HttpClient client = new HttpClient();

        private HTTPRequestService() { }

        public static HTTPRequestService Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<String> GetAsync(String url)
        {
            var responseString = "";
            HttpResponseMessage response;

            try
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);
                response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }

            }
            catch (Exception e)
            {
                throw;
            }

            return responseString;
        }

    }
}
