using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Services
{
    public class TrackingService:ITrackingService
    {
        private readonly ITrackingRepository trackingRepository;
        public TrackingService(ITrackingRepository trackingRepository)
        {
            this.trackingRepository = trackingRepository;
        }

        public List<TrackingModel> GetTrackings(int id, int page, int size)
        {
            return trackingRepository.GetTrackings(id, page, size);
        }

        public void UpdateTable(TrackingModel trackingModel)
        {
            trackingRepository.UpdateTable(trackingModel);
        }
    }
}
