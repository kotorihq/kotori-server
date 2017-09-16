namespace KotoriServer.Models
{
    /// <summary>
    /// Site error.
    /// </summary>
    /// <remarks>Maybe not needed?</remarks>
    public class SiteError
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}