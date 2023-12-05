
public class Student : Person
{
    public Student() : base()
    {
    }

    public Student(int _id, string _name) : base(_id, _name)
    {

    }

    public Student(string _name, string _username, string _password) :
        base(_name, _username, _password)
    {

    }

    public Student(int _id, string _name, string _username, string _password) :
        base(_id, _name, _username, _password)
    {

    }

    public Student(int _id, string _name, string _username) :
        base(_id, _name, _username)
    {

    }
}
