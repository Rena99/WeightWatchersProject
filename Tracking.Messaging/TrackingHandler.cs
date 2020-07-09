using AutoMapper;
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using Tracking.Services;


namespace Tracking.Messaging
{
    class TrackingHandler : IHandleMessages<AddTracking>
    {
        static ILog log = LogManager.GetLogger<TrackingHandler>();

        ITrackingService trackingService;
        IMapper mapper;
        public TrackingHandler(ITrackingService trackingService, IMapper mapper)
        {
            this.trackingService = trackingService;
            this.mapper = mapper;
        }
        public Task Handle(AddTracking message, IMessageHandlerContext context)
        {
            log.Info($"message #{message.Id} received");
            trackingService.UpdateTable(mapper.Map<TrackingModel>(message));
            return Task.CompletedTask;
        }
    }
}

