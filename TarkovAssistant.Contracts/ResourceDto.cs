using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class ResourceDto
    {
        public ResourceKind Kind { get; set; }
        public string Description { get; set; }        
        public string Picture { get; set; }
        public int Amount { get; set; }

        public ResourceDto()
        {
            Kind = ResourceKind.Screenshot;
            Description = string.Empty;
            Picture = string.Empty;
            Amount = 1;
        }

        public ResourceDto(ResourceEntity resource)
        { 
            Kind = resource.Kind;
            Description = resource.Description ?? string.Empty;
            Picture = resource.Hash ?? string.Empty;
            Amount = 1;
        }
    }
}
