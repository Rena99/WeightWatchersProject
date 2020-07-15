using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Data
{
    public class Tracking
    {
        public virtual int Id { get; set; }
        public virtual int CardId { get; set; }
        public virtual double Weight { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual bool Trend { get; set; }
        public virtual double BMI { get; set; }
        public virtual string Comments { get; set; }
    }
}
