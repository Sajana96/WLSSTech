namespace Client.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetApiBaseUrl(this IConfiguration configuration)
        {
            var value = configuration.GetValue<string>("ApiBaseUrl");
            if (string.IsNullOrEmpty(value))
            {
                throw new MissingFieldException("Api Base Url should be provided");
            }
            return value;
        }
    }
}
