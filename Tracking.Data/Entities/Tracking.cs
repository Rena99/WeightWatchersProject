using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Data
{
    public class Tracking
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public bool Trend { get; set; }
        public double BMI { get; set; }
        public string Comments { get; set; }
    }
}
