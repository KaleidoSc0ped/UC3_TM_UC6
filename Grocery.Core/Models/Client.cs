namespace Grocery.Core.Models;

public class Client : Model
{
    public Client(int id, string name, string emailAddress, string password)
        : base(id, name)
    {
        EmailAddress = emailAddress;
        Password = password;
    }

    public string EmailAddress { get; private set; }
    public string Password { get; private set; }
}