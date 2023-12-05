using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_StudentID;
    [SerializeField] TextMeshProUGUI m_StudentName;

    Student student;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            /*if(AudioManager.Singleton != null)
            {
                AudioManager.Singleton.Play("Button");
            }*/

            if (!UIManagerNew.Singleton.isOnClickRecommendations)
            {
                if (UIManagerNew.Singleton.isTeacherLogin)
                {
                    TeacherPortalManager.instance.SelectStudent(student);
                }
                else
                {
                    ParentPortalManager.instance.SelectStudent(student);
                }
            }
            else
            {
                if (UIManagerNew.Singleton.isTeacherLogin)
                {
                    TeacherPortalManager.instance.ShowStudentRecommendation(student);
                }
                else
                {
                    ParentPortalManager.instance.ShowStudentRecommendation(student);
                }
            }
        });
    }

    public void SetData(Student _student)
    {
        student = _student;
        m_StudentID.text = student.ID.ToString();
        m_StudentName.text = student.Name;
    }
}
    