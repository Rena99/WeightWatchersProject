using NServiceBus;

namespace Messages
{
    public class UpdateMeasureStatus:ICommand
    {
        public int Id { get; set; }
        public bool Succeeded { get; set; }
        public double BMI { get; set; }
    }
}
