﻿using AutoMapper;
using Measure.Services;
using Messages;

namespace Measure.WebApi
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<DTOMeasure, MeasureReceived>();
            CreateMap<MeasureModel, Data.Measure>();
        }
    }
}
