﻿using AutoMapper;
using Messages;
using Subscriber.Data.Entities;
using Subscriber.Services.Models;
using Subscriber.WebApi.DTO;

namespace Subscriber.Messaging
{
    class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<Data.Entities.Subscriber, SubscriberModel>();
            CreateMap<CardModel, Card>();
            CreateMap<SubscriberModel, Data.Entities.Subscriber>();
            CreateMap<Card, CardModel>();
            CreateMap<MeasureReceived, CardModel>();
            CreateMap<UserModel, DTOCard>()
                .ForMember(d => d.FirstName, a => a.MapFrom(s => s.Subscriber.FirstName))
                .ForMember(d => d.LastName, a => a.MapFrom(s => s.Subscriber.LastName))
                .ForMember(d => d.Height, a => a.MapFrom(s => s.Card.Height))
                .ForMember(d => d.Weight, a => a.MapFrom(s => s.Card.Weight))
                .ForMember(d => d.BMI, a => a.MapFrom(s => s.Card.BMI));
            CreateMap<DTOSubscriber, UserModel>()
               .ForPath(d => d.Subscriber.FirstName, a => a.MapFrom(s => s.FirstName))
               .ForPath(d => d.Subscriber.LastName, a => a.MapFrom(s => s.LastName))
               .ForPath(d => d.Card.Height, a => a.MapFrom(s => s.Height))
               .ForPath(d => d.Subscriber.Email, a => a.MapFrom(s => s.Email))
               .ForPath(d => d.Subscriber.Password, a => a.MapFrom(s => s.Password));
        }

    }
}
