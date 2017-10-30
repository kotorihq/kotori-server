namespace KotoriServer.Models
{
    // TODO: Maybe not needed?
    /// <summary>
    /// Site error.
    /// </summary>
    public class SiteError
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}