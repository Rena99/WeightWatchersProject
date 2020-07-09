namespace Measure.Services
{
    public class MeasureService : IMeasureService
    {
        private readonly IMeasureRepository measureRepository;
        public MeasureService(IMeasureRepository measureRepository)
        {
            this.measureRepository = measureRepository;
        }
        public int PostMeasure(MeasureModel measure)
        {
            return measureRepository.PostMeasure(measure);
        }

        public void UpdateStatus(int measureId, bool succeeded)
        {
            measureRepository.UpdateStatus(measureId, succeeded);
        }
    }
}
