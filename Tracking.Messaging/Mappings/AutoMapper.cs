using AutoMapper;
using Messages;
using Tracking.Services;
using Tracking.WebApi;

namespace Tracking.Messaging
{
    class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<Data.Tracking, TrackingModel>();
            CreateMap<TrackingModel, DTOTracking>();
            CreateMap<TrackingModel, Data.Tracking>();
            CreateMap<AddTracking, TrackingModel>();
        }

    }
}
