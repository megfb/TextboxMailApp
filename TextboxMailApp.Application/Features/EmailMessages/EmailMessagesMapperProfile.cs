using AutoMapper;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.EmailMessages
{
    public class EmailMessagesMapperProfile : Profile
    {
        public EmailMessagesMapperProfile()
        {
            CreateMap<EmailMessage, EmailMessagesDto>();
            CreateMap<EmailMessagesDto, EmailMessage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToUniversalTime()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.HasValue
                    ? src.UpdatedAt.Value.ToUniversalTime()
                    : (DateTime?)null))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToUniversalTime()));
        }
    }
}
