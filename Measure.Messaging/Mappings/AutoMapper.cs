using AutoMapper;
using Measure.Services;
using Measure.WebApi;
using Messages;

namespace Measure.Messaging
{
    class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<DTOMeasure, MeasureReceived>();
            CreateMap<MeasureModel, Data.Measure>();
        }
    }
}
