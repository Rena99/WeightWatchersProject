using AutoMapper;
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using Subscriber.Services.Interfaces;
using Subscriber.WebApi.DTO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Subscriber.Messaging
{
    class NewMeasureHandler : IHandleMessages<MeasureReceived>
    {
        static ILog log = LogManager.GetLogger<NewMeasureHandler>();

        ICardService cardService;
        IMapper mapper;
        public NewMeasureHandler(ICardService cardService, IMapper mapper)
        {
            this.cardService = cardService;
            this.mapper = mapper;
        }
        public Task Handle(MeasureReceived message, IMessageHandlerContext context)
        {
            log.Info($"message #{message.Id} received");
            bool succeeded=cardService.UpdateCard(message.CardId, (double)message.Weight);
            var BMI = succeeded ? mapper.Map<DTOCard>(cardService.GetCard(message.CardId)).BMI : 0;
            var updateMeasure = new UpdateMeasureStatus
            {
                Id = message.Id,
                Succeeded=succeeded,
                BMI = BMI
            };
            return context.Send(updateMeasure);
        }
    }
}

