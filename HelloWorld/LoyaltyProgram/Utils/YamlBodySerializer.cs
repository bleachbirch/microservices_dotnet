using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using YamlDotNet.Serialization;

namespace LoyaltyProgram.Utils
{
    public class YamlBodySerializer : IResponseProcessor
    {
        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                yield return new Tuple<string, MediaRange>("yaml", new MediaRange("application/yaml"));
            }
        }

        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context) =>
            requestedMediaRange.Subtype.ToString().EndsWith("yaml") ?
            new ProcessorMatch
            {
                ModelResult = MatchResult.DontCare,
                RequestedContentTypeResult = MatchResult.NonExactMatch
            } :
            ProcessorMatch.None;

        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context) =>
            new Response
            {
                Contents =  stream =>
                {
                    var yamlSerializer = new Serializer();
                    var writer = new StreamWriter(stream);
                    yamlSerializer.Serialize(writer, model);
                    writer.Flush();
                },
                ContentType = "application/yaml"
            };
    }
}
