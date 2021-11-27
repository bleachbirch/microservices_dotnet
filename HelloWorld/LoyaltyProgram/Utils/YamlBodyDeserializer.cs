using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using YamlDotNet.Serialization;

namespace LoyaltyProgram.Utils
{
    public class YamlBodyDeserializer : IBodyDeserializer
    {
        public bool CanDeserialize(MediaRange mediaRange, BindingContext context)
        {
            return mediaRange.Subtype.ToString().EndsWith("yaml");
        }

        public object Deserialize(MediaRange mediaRange, Stream bodyStream, BindingContext context)
        {
            var yamlDeserializer = new Deserializer();
            var reader = new StreamReader(bodyStream);
            return yamlDeserializer.Deserialize(reader, context.DestinationType);
        }
    }
}
