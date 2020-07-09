using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class AddTracking: ICommand
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }

    }
}
