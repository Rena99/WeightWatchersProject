﻿using AutoMapper;
using Subscriber.Data.Entities;
using Subscriber.Models;
using Subscriber.Services.Interfaces;
using Subscriber.Services.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Subscriber.Data
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly WeightWatchers context;
        private readonly IMapper _mapper;
        public SubscriberRepository(IMapper mapper, WeightWatchers context)
        {
            _mapper = mapper;
            this.context = context;
        }
       
        public int checkSubscriberEmail(string email, string password)
        {
            Entities.Subscriber subscriber = new Entities.Subscriber();
            foreach (var item in context.Subscribers)
            {
                if (item.Email==email&&AreEqual(password, item.Password, ConfigurationManager.AppSettings["salt"]))
                {
                    subscriber = item;
                    break;
                }
            }
            foreach (var card in context.Cards)
            {
                if (card.SubscriberId == subscriber.Id)
                {
                    return card.Id;
                }
            }
            return 0;
        }

        public bool NewSubscriber(UserModel mUser)
        {
            foreach (var item in context.Subscribers)
            {
                if (item.Email == mUser.Subscriber.Email)
                {
                    return false;
                }
            }
            Entities.Subscriber subscriber = _mapper.Map<Entities.Subscriber>(mUser.Subscriber);
            subscriber.Password = GenerateHash(subscriber.Password, ConfigurationManager.AppSettings["salt"]);
            subscriber.Id = Guid.NewGuid();
            Card card = _mapper.Map<Card>(mUser.Card);
            card.OpenDate = DateTime.Now;
            card.UpdateDate = DateTime.Now;
            card.SubscriberId = subscriber.Id;
            context.Subscribers.Add(subscriber);
            context.Cards.Add(card);
            context.SaveChanges();
            return true;
        }
        private string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool AreEqual(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }

       
    }
}
