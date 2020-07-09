namespace Measure.Services
{
    public interface IMeasureService
    {
        int PostMeasure(MeasureModel measure);
        void UpdateStatus(int measureId, bool succeeded);
    }
}
