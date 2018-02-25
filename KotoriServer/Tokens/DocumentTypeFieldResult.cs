namespace KotoriServer.Tokens
{
    public class DocumentTypeFieldResult : IResult
    {
        public string Field { get; set; }

        public DocumentTypeFieldResult(string field)
        {
            Field = field;
        }
    }
}
