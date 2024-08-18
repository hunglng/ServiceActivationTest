namespace ServiceActivationTest.Tenant
{
    public interface ITenantInfo
    {
        string TenantId { get; set; }
    }

    public class TenantInfo : ITenantInfo
    {
        public string TenantId { get; set; }
    }
}
