using Measure.Services;
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Measure.Messaging
{
    class AddMeasureHandler : Saga<SagaData>,
                IAmStartedByMessages<MeasureReceived>,
                IAmStartedByMessages<UpdateMeasureStatus>
    {
        static ILog log = LogManager.GetLogger<AddMeasureHandler>();

        private readonly IMeasureService measureService;
        public AddMeasureHandler(IMeasureService measureService)
        {
            this.measureService = measureService;
        }
        public Task Handle(MeasureReceived message, IMessageHandlerContext context)
        {
            log.Info($"Message #{message.Id} received");
            Data.isMeasureReceived = true;
            Data.CardId = message.CardId;
            Data.Weight = (double)message.Weight;
            MeasureModel measureModel = new MeasureModel()
            {
                Weight = message.Weight,
                CardId = message.CardId,
                Status = "InProcess"
            };
            Data.MeasureId = measureService.PostMeasure(measureModel);
            return AddMeasure(context);
        }

        public Task Handle(UpdateMeasureStatus message, IMessageHandlerContext context)
        {
            log.Info($"Message #{message.Id} successful");
            Data.Succeeded = true;
            Data.BMI = message.BMI;
            Data.MessageSucceeded = message.Succeeded;
            return AddMeasure(context);
        }
        private async Task AddMeasure(IMessageHandlerContext context)
        {
            if (Data.isMeasureReceived && Data.Succeeded)
            {
                log.Info($"Message #{Data.SagaId} completed");
                measureService.UpdateStatus(Data.MeasureId, Data.MessageSucceeded);
                if (Data.MessageSucceeded)
                {
                    await context.Send(new AddTracking()
                    {
                        CardId = Data.CardId,
                        Weight=Data.Weight,
                        BMI=Data.BMI
                    });
                }
                MarkAsComplete();
            }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<MeasureReceived>(message => message.Id.ToString())
                .ToSaga(sagaData => sagaData.SagaId);
            mapper.ConfigureMapping<UpdateMeasureStatus>(message => message.Id.ToString())
                .ToSaga(sagaData => sagaData.SagaId);
        }

    }

    public class SagaData : ContainSagaData
    {
        public string SagaId { get; set; }
        public bool isMeasureReceived { get; set; }
        public bool Succeeded { get; set; }
        public int CardId { get; set; }
        public int MeasureId { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public bool MessageSucceeded { get; set; }
    }

}
