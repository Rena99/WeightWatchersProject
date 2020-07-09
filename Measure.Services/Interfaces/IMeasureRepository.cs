namespace Measure.Services
{
    public interface IMeasureRepository
    {
        int PostMeasure(MeasureModel measure);
        void UpdateStatus(int measureId, bool succeeded);
    }
}
