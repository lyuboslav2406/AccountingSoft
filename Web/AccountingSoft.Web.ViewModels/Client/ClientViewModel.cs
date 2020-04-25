namespace AccountingSoft.Web.ViewModels.Client
{
    using AccountingSoft.Services.Mapping;
    using AutoMapper;
    using AccountingSoft.Data.Models;
    using System;

    public class ClientViewModel : IMapFrom<Client>, IMapTo<Client>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string EIK { get; set; }

        public bool DDS { get; set; }

        public Guid Id { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Client, ClientViewModel>();
        }
    }
}
