using AutoMapper;
using Subscriber.Data.Entities;
using Subscriber.Models;
using Subscriber.Services.Interfaces;
using Subscriber.Services.Models;
using System;
using System.Linq;

namespace Subscriber.Data
{
    public class CardRepository : ICardRepository
    {
        private readonly WeightWatchers weightWatchers;
        private readonly IMapper mapper;
        public CardRepository(WeightWatchers weightWatchers, IMapper mapper)
        {
            this.weightWatchers = weightWatchers;
            this.mapper = mapper;
        }
        public UserModel GetCard(int id)
        {
            Card card = weightWatchers.Cards.FirstOrDefault(c => c.Id == id);
            CardModel mCard = mapper.Map<CardModel>(card);
            SubscriberModel subscriber = mapper.Map<SubscriberModel>(weightWatchers.Subscribers.FirstOrDefault(s => s.Id == card.SubscriberId));
            return new UserModel()
            {
                Card = mCard,
                Subscriber = subscriber
            };
        }
        public bool UpdateCard(int id, double weight)
        {
            Card card = weightWatchers.Cards.FirstOrDefault(c => c.Id == id);
            if (card == null) return false;
            card.UpdateDate = DateTime.Now;
            card.Weight = weight;
            card.BMI = CalculateBMI(card.Height, card.Weight);
            weightWatchers.SaveChanges();
            return true;
        }

        private double CalculateBMI(double height, double weight)
        {
            return weight / Math.Pow(height, 2);
        }
    }
}
