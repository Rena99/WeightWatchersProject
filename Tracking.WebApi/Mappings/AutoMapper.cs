using AutoMapper;
using Messages;
using Tracking.Services;
using Tracking.WebApi;

namespace Tracking.WebApi
{
    class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<Tracking.Data.Tracking, TrackingModel>();
            CreateMap<TrackingModel, DTOTracking>();
            CreateMap<TrackingModel, Tracking.Data.Tracking>();
            CreateMap<AddTracking, TrackingModel>();
        }

    }
}
