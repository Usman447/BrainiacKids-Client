using System.Linq;

namespace Network
{
    public static class MessageExtensions
    {
        #region Teacher

        public static Message Add(this Message message, Teacher value) => AddTeacher(message, value);

        public static Message AddTeacher(this Message message, Teacher value)
        {
            message.AddInt(value.ID);
            message.AddString(value.Name);
            message.AddInts(value.GetStudentsID().ToArray());
            return message;
        }

        public static Teacher GetTeacher(this Message message)
        {
            Teacher teacher = new Teacher(message.GetInt(), message.GetString());
            teacher.SetStudentIDs(message.GetInts().ToList());

            return teacher;
        }
        #endregion

        #region Parent

        public static Message Add(this Message message, Parent value) => AddParent(message, value);

        public static Message AddParent(this Message message, Parent value)
        {
            message.AddInt(value.ID);
            message.AddString(value.Name);
            message.AddInts(value.GetStudentsID().ToArray());
            return message;
        }

        public static Parent GetParent(this Message message)
        {
            Parent parent = new Parent(message.GetInt(), message.GetString());
            parent.SetStudentIDs(message.GetInts().ToList());

            return parent;
        }
        #endregion


        #region Student

        public static Message Add(this Message message, Student value) => AddStudent(message, value);

        public static Message AddStudent(this Message message, Student value)
        {
            message.AddInt(value.ID);
            message.AddString(value.Name);
            message.AddString(value.Username);
            return message;
        }

        public static Student GetStudent(this Message message)
        {
            Student student = new Student(message.GetInt(), message.GetString(),
                message.GetString());

            return student;
        }
        #endregion

        #region Login

        public static Message Add(this Message message, string username,
            string password) => AddLogin(message, username, password);

        public static Message AddLogin(this Message message, string username, string password)
        {
            message.AddStrings(new string[] { username, password });
            return message;
        }

        public static string[] GetLogin(this Message message)
        {
            return message.GetStrings();
        }

        #endregion
    }
}