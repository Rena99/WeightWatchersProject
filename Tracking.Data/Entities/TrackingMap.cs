using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Data.Entities
{
    public class TrackingMap : ClassMapping<Tracking>
    {
        public TrackingMap()
        {
            Id(x => x.Id, x =>
            {
                x.Generator(Generators.Identity);
                x.Type(NHibernateUtil.Int32);
                x.Column("Id");
            });

            Property(b => b.CardId, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.NotNullable(true);
            });

            Property(b => b.Weight, x =>
            {
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });

            Property(b => b.Date, x =>
            {
                x.Type(NHibernateUtil.DateTime);
                x.NotNullable(true);
            });

            Property(b => b.Trend, x =>
            {
                x.Type(NHibernateUtil.Boolean);
                x.NotNullable(true);
            });

            Property(b => b.BMI, x =>
            {
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });
            Property(b => b.Comments, x =>
            {
                x.Length(500);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
            });
            Table("Trend");
        }
    }
}
