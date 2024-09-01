namespace UserManager.Common.Configs
{
    public class JwtConfigurations
    {
        public string Secret { get; set; }
        public int TokenExpiryInMins { get; set; }
        public string Issuer { get; set; }
    }
}
