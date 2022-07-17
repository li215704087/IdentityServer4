using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // 客户端认证
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(
            //        new ClientCredentialsTokenRequest
            //        {
            //            Address = disco.TokenEndpoint,
            //            ClientId = "sample_client",
            //            ClientSecret = "sample_client_secret"
            //        }
            //    );
            
            // 资源拥有者认证
            var tokenResponse = await client.RequestPasswordTokenAsync(
                    new PasswordTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "sample_pass_client",
                        ClientSecret = "sample_client_secret",
                        UserName="admin",
                        Password="123"
                    }
                );

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);


            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.PostAsync("http://localhost:5000/IdentityServer", null);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }


            Console.ReadKey();
        }
    }
}
