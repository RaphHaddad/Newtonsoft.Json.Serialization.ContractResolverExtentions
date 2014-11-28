namespace Newtonsoft.Json.Serialization.ContractResolverExtentions
{
    public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            var startUnderscores = System.Text.RegularExpressions.Regex.Match(propertyName, @"^_+");
            return startUnderscores + System.Text.RegularExpressions.Regex
                .Replace(propertyName, @"([A-Z])", "_$1").ToLower().TrimStart('_');
        }
    }
}
