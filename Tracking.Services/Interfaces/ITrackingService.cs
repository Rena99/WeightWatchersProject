using System.Collections.Generic;

namespace Tracking.Services
{
    public interface ITrackingService
    {
        void UpdateTable(TrackingModel trackingModel);
        List<TrackingModel> GetTrackings(int id, int page, int size);
        TrackingModel GetTracking(int id);
    }
}
