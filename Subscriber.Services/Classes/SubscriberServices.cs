using Subscriber.Services.Interfaces;
using Subscriber.Services.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Subscriber.Services
{
    public class SubscriberServices : ISubscriberServices
    {
        private readonly ISubscriberRepository subscriberRepository;
        public SubscriberServices(ISubscriberRepository subscriberRepository)
        {
            this.subscriberRepository = subscriberRepository;
        }
        public int checkSubscriberEmail(string email,  string password)
        {
           return subscriberRepository.checkSubscriberEmail(email, password);
        }

        public bool NewSubscriber(UserModel mUser)
        {
            if (subscriberRepository.NewSubscriber(mUser))
            {
                var fromAddress = new MailAddress("renaMarkiewitz@gmail.com", "WeightWatchers");
                var toAddress = new MailAddress(mUser.Subscriber.Email, mUser.Subscriber.FirstName + " " + mUser.Subscriber.LastName);
                const string fromPassword = "Rena99@Google";
                const string subject = "You have successfully subscribed to weight watchers";
                const string body = "thank you for subscribing to weight watchers";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                return true;
            }
            return false;
        }

       
    }
}
