namespace BSMART
{
    internal class ResidentSearchSuggestion
    {
        public string LookupText { get; set; } = "";
        public string DisplayName { get; set; } = "";

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
