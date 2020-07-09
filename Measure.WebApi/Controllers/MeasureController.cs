using AutoMapper;
using Messages;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Measure.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasureController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMessageSession messageSession;
        public MeasureController(IMapper mapper, IMessageSession messageSession)
        {
            this.mapper = mapper;
            this.messageSession = messageSession;
        }
        [HttpPost]
        public void PostMeasure([FromBody] DTOMeasure measure)
        {
            messageSession.Publish(mapper.Map<MeasureReceived>(measure)).ConfigureAwait(false);
        }
    }
}
