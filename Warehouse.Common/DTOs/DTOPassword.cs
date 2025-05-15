namespace Warehouse.Common.DTOs
{
    public class HashPasswordRequest
    {
        public string Password { get; set; }
    }

    public class VerifyPasswordRequest
    {
        public string Password { get; set; }
        public string StoredHash { get; set; }
    }
}
