namespace Newtonsoft.Json.Serialization.ContractResolverExtentions
{
    public class SnakeCaseIntegerSeperatedPropertyNamesContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            var startUnderscores = System.Text.RegularExpressions.Regex.Match(propertyName, @"^_+");
            return startUnderscores + System.Text.RegularExpressions.Regex.Replace(propertyName, @"([A-Z0-9])", "_$1").ToLower().TrimStart('_');
        }
    }	
}
