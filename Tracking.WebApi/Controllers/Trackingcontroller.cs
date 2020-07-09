using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tracking.Services;

namespace Tracking.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class Trackingcontroller : ControllerBase
    {
        private readonly ITrackingService trackingService;
        private readonly IMapper mapper;
        public Trackingcontroller(ITrackingService trackingService, IMapper mapper)
        {
            this.trackingService = trackingService;
            this.mapper = mapper;
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
    }
}
