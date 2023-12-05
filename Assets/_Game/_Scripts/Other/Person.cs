
public class Person
{
    int _id;
    string username;
    string password;
    string name;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public int ID
    {
        get => _id;
        set => _id = value;
    }

    public string Username
    {
        get => !string.IsNullOrEmpty(username) ? username : string.Empty;
        set => username = value;
    }

    public string Password
    {
        get => password;
        set => password = value;
    }

    protected Person()
    {
        ID = -1;
        Name = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
    }

    protected Person(int _id, string _name)
    {
        ID = _id;
        Name = _name;
    }

    protected Person(int _id, string _name, string _username)
    {
        ID = _id;
        Name = _name;
        Username = _username;
    }

    protected Person(int _id, string _name, string _username, string _password)
    {
        ID = _id;
        Name = _name;
        Username = _username;
        Password = _password;
    }

    protected Person(string _name, string _username, string _password)
    {
        ID = -1;
        Name = _name;
        Username = _username;
        Password = _password;
    }
}
