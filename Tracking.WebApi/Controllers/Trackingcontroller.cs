using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tracking.Data;
using Tracking.Services;

namespace Tracking.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class Trackingcontroller : ControllerBase
    {
        private readonly ITrackingService trackingService;
        private readonly IMapper mapper;
        private readonly IMapperSession _session;

      
        
        public Trackingcontroller(ITrackingService trackingService, IMapper mapper, IMapperSession session)
        {
            this.trackingService = trackingService;
            this.mapper = mapper;
            _session = session;

        }
        [HttpGet("{id}")]
        public List<DTOTracking> GetTrackings(int id, [FromQuery] int page, [FromQuery] int size)
        {
            List<TrackingModel> trackingModels = trackingService.GetTrackings(id, page, size);
            List<DTOTracking> trackings = new List<DTOTracking>();
            foreach (var item in trackingModels)
            {
                trackings.Add(mapper.Map<DTOTracking>(item));
            }
            return trackings;
        }

        [HttpGet("ById/{id}")]
        public DTOTracking GetTracking(int id)
        {
            return mapper.Map<DTOTracking>(trackingService.GetTracking(id));
        }
    }
}
