using Twilio.Clients;
using Twilio.Http;

namespace VehiclesModule.Services
{
    public class TwilioClientts : ITwilioRestClient
    {
        private readonly ITwilioRestClient innerClient;
        public TwilioClientts(IConfiguration config, System.Net.Http.HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("X-Custom-Header", "Custom TwilloRestClient-Demo");
            innerClient = new TwilioRestClient(
                "ACb31801cd4568f00fd2ba96fba6bab542",
                "524e5bf59f6ef9e79da0247a784df63c",
                httpClient: new SystemNetHttpClient(httpClient)
                );
        }
        public string AccountSid => innerClient.AccountSid;

        public string Region => innerClient.Region;

        public Twilio.Http.HttpClient HttpClient => innerClient.HttpClient;

        public Response Request(Request request) => innerClient.Request(request);


        public Task<Response> RequestAsync(Request request) => innerClient.RequestAsync(request);

    }
}