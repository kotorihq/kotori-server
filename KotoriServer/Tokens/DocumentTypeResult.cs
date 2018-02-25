using System.Collections.Generic;
using System.Linq;
using KotoriCore.Domains;

namespace KotoriServer.Tokens
{
    public class DocumentTypeResult : IResult
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public IEnumerable<DocumentTypeFieldResult> Fields { get; set; }

        public DocumentTypeResult(string id, string type, IEnumerable<DocumentTypeFieldResult> fields)
        {
            Id = id;
            Type = type;
            Fields = fields;
        }

        public static implicit operator DocumentTypeResult(SimpleDocumentType simpleDocumentType)
        {
            return new DocumentTypeResult
            (
                simpleDocumentType.Identifier,
                simpleDocumentType.Type,
                simpleDocumentType.Fields.Select(f => new DocumentTypeFieldResult(f))
            );
        }
    }
}
