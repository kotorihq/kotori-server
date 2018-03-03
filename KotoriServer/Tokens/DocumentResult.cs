using System;
using KotoriCore.Domains;

namespace KotoriServer.Tokens
{
    /// <summary>
    /// Document result.
    /// </summary>
    public class DocumentResult : IResult
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the slug.
        /// </summary>
        /// <value>The slug.</value>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        public dynamic Meta { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the modified.
        /// </summary>
        /// <value>The modified.</value>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Gets or sets the draft.
        /// </summary>
        /// <value>The draft.</value>
        public bool? Draft { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public long Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KotoriServer.Tokens.DocumentResult"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="slug">Slug.</param>
        /// <param name="meta">Meta.</param>
        /// <param name="content">Content.</param>
        /// <param name="date">Date.</param>
        /// <param name="modified">Modified.</param>
        /// <param name="draft">Draft.</param>
        /// <param name="version">Version.</param>
        public DocumentResult(string id, string slug, dynamic meta, string content, DateTime? date, DateTime? modified, bool? draft, long version)
        {
            Id = id;
            Slug = slug;
            Meta = meta;
            Content = content;
            Date = date;
            Modified = modified;
            Draft = draft;
            Version = version;
        }

        public static implicit operator DocumentResult(SimpleDocument doc)
        {
            return new DocumentResult
                (
                    doc.Identifier,
                    doc.Slug,
                    doc.Meta,
                    doc.Content,
                    doc.Date,
                    doc.Modified,
                    doc.Draft,
                    doc.Version
                );
        }
    }
}
