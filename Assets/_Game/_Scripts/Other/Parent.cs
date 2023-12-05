using System.Collections.Generic;

public class Parent : Person
{
    List<int> childsID = null;

    public Parent() : base()
    {
        childsID = new List<int>();
    }

    public Parent(int _id, string _name) : base(_id, _name)
    {
        childsID = new List<int>();
    }

    public Parent(string _name, string _username, string _password, List<int> _childId) :
        base(_name, _username, _password)
    {
        childsID = _childId;
    }

    public void SetStudentIDs(List<int> _childId)
    {
        this.childsID = _childId;
    }

    public List<int> GetStudentsID()
    {
        return childsID != null ? childsID : null;
    }
}
