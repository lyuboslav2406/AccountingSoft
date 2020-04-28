namespace AccountingSoft.Web.ViewModels.Client
{
    using AccountingSoft.Services.Mapping;
    using AutoMapper;
    using AccountingSoft.Data.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ClientViewModel : IMapFrom<Client>, IMapTo<Client>, IHaveCustomMappings
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(9)]
        [MinLength(9)]
        public string EIK { get; set; }

        public bool DDS { get; set; }

        public Guid Id { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Client, ClientViewModel>();
        }
    }
}
