namespace NetCore.Shared.Configurations
{
    public class AuthenticationServerConfiguration
    {
        public string AccessKey { get; set; }
        public string Audience { get; set; }
        public string CertificatePassword { get; set; }
        public string CertificatePath { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
    }
}
