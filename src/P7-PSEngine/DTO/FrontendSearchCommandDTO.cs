public struct FrontendSearchCommandDTO
{
    public string Keyword { get; set; }
    public string Explanation { get; set; }
    public FrontendSearchCommandDTO(string keyword, string explanation)
    {
        Keyword = keyword;
        Explanation = explanation;
    }
}