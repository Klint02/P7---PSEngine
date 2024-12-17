namespace P7_PSEngine.Model;

public class CloudService
{

    public int Id { get; set; }


    public string ServiceType { get; set; }

    public int UserId { get; set; }


    public User User { get; set; }


    public string UserDefinedServiceName { get; set; }


    public string refresh_token { get; set; }


}