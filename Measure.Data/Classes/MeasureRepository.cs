using System;
using System.Linq;
using AutoMapper;
using Measure.Services;

namespace Measure.Data
{
    public class MeasureRepository : IMeasureRepository
    {
        private readonly MeasureContext context;
        private readonly IMapper mapper;
        public MeasureRepository(MeasureContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public int PostMeasure(MeasureModel measure)
        {
            Measure measure1 = mapper.Map<Measure>(measure);
            measure1.Date = DateTime.Now;
            Measure meas = context.Measures.FirstOrDefault(m => m.CardId == measure.CardId);
            if (meas != null)
            {
                meas.Date = measure1.Date;
                meas.Status = measure1.Status;
                meas.Weight = measure1.Weight;
            }
            else
            {
                context.Measures.Add(measure1);
            }
            context.SaveChanges();
            meas = context.Measures.FirstOrDefault(m => m.CardId == measure.CardId);
            return meas.Id;
        }

        public void UpdateStatus(int measureId, bool succeeded)
        {
            context.Measures.FirstOrDefault(m => m.Id == measureId).Status = succeeded.ToString();
            context.SaveChanges();
        }
    }
}
