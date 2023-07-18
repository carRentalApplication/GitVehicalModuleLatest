using CarRentalEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;
using Twilio.Types;

namespace VehiclesModule.Services
{
    public class TwilioServices
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _phoneNumber;
        private readonly ITwilioRestClient restClient;
        public TwilioServices(IConfiguration configuration, ITwilioRestClient restClient)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _phoneNumber = configuration["Twilio:PhoneNumber"];

            TwilioClient.Init(_accountSid, _authToken);
            this.restClient = restClient;
        }
        public string SendPendingSMS(Booking booking)
        {
            // Remove the "+91" prefix from the phone number
            string To = booking.TravellerNumber.ToString();
            string message1 = "\nDear " + booking.TravellerName + ",\n Your Request Received successfully." +
                "We will Review your application and contact you later";

            var message = MessageResource.Create(
                to: new PhoneNumber(To),
                from: new PhoneNumber(_phoneNumber),
                body: message1,
                client: this.restClient
            );

            return message.Sid;
        }

        public void SendConfirmationSMS(Booking booking)
        {
            string To = "+91 " + booking.TravellerNumber.ToString();
            string message1;
            if (booking.Status.StatusName == "accept")
            {
                message1 = "\n\n Dear " + booking.TravellerName + ",\n Your Request is approved successfully.\n" +
                    "Our Driver will pickup you at " + booking.PickUpAddress
                    + "\nThis service is available for you from " + booking.PickUpDate + " to " + booking.DropDate;
            }
            else
            {
                message1 = "\n Dear " + booking.TravellerName + ",\n Sorry Your Request is cancelled";
            }
            var message = MessageResource.Create(
                to: new PhoneNumber(To),
                from: new PhoneNumber(_phoneNumber),
                body: message1,
                client: this.restClient
                );
        }
    }
}