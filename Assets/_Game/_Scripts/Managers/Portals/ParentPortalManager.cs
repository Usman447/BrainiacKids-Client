using Network;
using NetworkDataManager;
using ScrollView;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParentPortalManager : MonoBehaviour
{
    [SerializeField] GameObject m_StudentsIDPanel;
    [SerializeField] GameObject m_IndividualSubjectSelector;

    [Header("Scrolling and Prefab")]

    [SerializeField] GameObject m_StudentInfoPanel;
    [SerializeField] ScrollManager m_ScollManager;

    [Header("Recommendation Stuff")]
    [SerializeField] GameObject m_RecommendationPanel;
    [SerializeField] TextMeshProUGUI m_RecommentationText;

    //List<Student> students;
    public static ParentPortalManager instance;
    List<GameObject> studentInfoPanels;

    private void Awake()
    {
        instance = this;
        //students = new List<Student>();
        studentInfoPanels = new List<GameObject>();
    }

    private void OnEnable()
    {
        SendRequestForStudentsNames();
    }

    void SendRequestForStudentsNames()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, ClientToServer.StudentIDAndName);
        sendMessage.AddInts(UIManagerNew.Singleton.Parent.GetStudentsID().ToArray());
        NetworkManager.Singleton.Client.Send(sendMessage);
    }


    public void OnClickSelectRecommendationButton()
    {
        UIManagerNew.Singleton.isOnClickRecommendations = true;
        PlotStudentInfoIDs();
    }

    public void ShowStudentRecommendation(Student student)
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.StudentRecommendation);
        sendMessage.AddStudent(student);
        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    public void OnClickShowStudentIDs()
    {
        UIManagerNew.Singleton.isOnClickRecommendations = false;
        PlotStudentInfoIDs();
    }

    void PlotStudentInfoIDs()
    {
        m_StudentsIDPanel.SetActive(true);
        foreach (Student s in UIManagerNew.Singleton.students)
        {
            GameObject studentInfoPanel = m_ScollManager.AddToFront(m_StudentInfoPanel);
            studentInfoPanel.GetComponent<StudentUI>().SetData(s);
            studentInfoPanels.Add(studentInfoPanel);
        }
    }

    public void OnClickReturnToOptionSelectionPanel()
    {
        foreach (GameObject panel in studentInfoPanels)
            Destroy(panel);
        studentInfoPanels.Clear();
        m_StudentsIDPanel.SetActive(false);
    }


    Student selectedStudent = null;

    public void SelectStudent(Student _student)
    {
        selectedStudent = _student;
        m_IndividualSubjectSelector.SetActive(true);
    }

    public void OnClickCloseIndividualSubjectSelector()
    {
        m_IndividualSubjectSelector.SetActive(false);
        selectedStudent = null;
    }

    // ///////////////  Individual English Panel ////////////////// //
    public void SendEnglishIndividualProgressRequest()
    {
        if (selectedStudent != null)
        {
            Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ISD_English);
            sendMessage.AddInt(selectedStudent.ID);
            sendMessage.AddString(selectedStudent.Name);
            NetworkManager.Singleton.Client.Send(sendMessage);
        }
    }

    // ///////////////  Individual Maths Panel ////////////////// //


    public void SendMathsIndividualProgressRequest()
    {
        if (selectedStudent != null)
        {
            Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ISD_Maths);
            sendMessage.AddInt(selectedStudent.ID);
            sendMessage.AddString(selectedStudent.Name);
            NetworkManager.Singleton.Client.Send(sendMessage);
        }
    }


    // ///////////////  Individual Urdu Panel ////////////////// //

    public void SendUrduIndividualProgressRequest()
    {
        if (selectedStudent != null)
        {
            Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ISD_Urdu);
            sendMessage.AddInt(selectedStudent.ID);
            sendMessage.AddString(selectedStudent.Name);
            NetworkManager.Singleton.Client.Send(sendMessage);
        }
    }
}
