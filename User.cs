namespace Auth;
public class User
{
    public string id { get; set; }
    public string partitionKey { get; set; }
    public string Restaurant { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}