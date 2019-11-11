namespace Core.Dto
{
    public class GetEntriesResponse : UseCaseResponse
    {
        public Entry[] Entries { get; set; }

        public GetEntriesResponse(Entry[] entries, bool success, string message)
            : base(success, message)
        {
            Entries = entries;
        }
    }
}
