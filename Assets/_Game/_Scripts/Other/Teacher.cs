using System.Collections.Generic;

public class Teacher : Person
{
    List<int> studentsID = null;

    public Teacher() : base()
    {
        studentsID = new List<int>();
    }

    public Teacher(int _id, string _name) : base(_id, _name)
    {
        studentsID = new List<int>();
    }

    public Teacher(string _name, string _username, string _password) :
        base(_name, _username, _password)
    {
        studentsID = new List<int>();
    }

    public void SetStudentIDs(List<int> _studentIDs)
    {
        this.studentsID = _studentIDs;
    }

    public List<int> GetStudentsID()
    {
        return studentsID != null ? studentsID : null;
    }

    public void AddStudent(int _studentId)
    {
        if (!studentsID.Contains(_studentId))
            studentsID.Add(_studentId);
    }

    public void RemoveStudent(int _studentId)
    {
        if (studentsID.Contains(_studentId))
            studentsID.Remove(_studentId);
    }
}
