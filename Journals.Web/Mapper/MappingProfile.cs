using AutoMapper;
using Journals.Model;
using Journals.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Journals.Web.Mapper
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Journal, JournalViewModel>();
                cfg.CreateMap<JournalViewModel, Journal>();

                cfg.CreateMap<Journal, JournalUpdateViewModel>();
                cfg.CreateMap<JournalUpdateViewModel, Journal>();

                cfg.CreateMap<Journal, SubscriptionViewModel>();
                cfg.CreateMap<SubscriptionViewModel, Journal>();
            });

            return config;
        }
    }
}