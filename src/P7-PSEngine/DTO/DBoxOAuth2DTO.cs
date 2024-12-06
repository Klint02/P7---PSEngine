namespace P7_PSEngine.DTO;

public struct DBoxOAuth2DTO 
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
    public string scope { get; set; }
    public string uid { get; set; }
    public string account_id { get; set; }
}