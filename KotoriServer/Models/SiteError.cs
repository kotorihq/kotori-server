namespace KotoriServer.Models
{
    /// <summary>
    /// Site error.
    /// </summary>
    /// <remarks>Maybe not neede?</remarks>
    public class SiteError
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}