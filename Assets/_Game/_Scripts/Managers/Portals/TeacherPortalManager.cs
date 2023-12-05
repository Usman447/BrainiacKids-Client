using Network;
using NetworkDataManager;
using ScrollView;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TeacherPortalManager : MonoBehaviour
{
    [Header("Overall Progress Stuff")]
    [SerializeField] GameObject m_OverallProgressPanelSelector;
    [SerializeField] GameObject m_OverallProgressPanel;
    [SerializeField] GameObject m_OverallClassProgressSelector;

    [Header("Individual Progress Stuff")]
    [SerializeField] GameObject m_StudentsIDPanel;
    [SerializeField] GameObject m_IndividualSubjectSelector;

    [SerializeField] GameObject m_EnglishProgressPanel;
    [SerializeField] GameObject m_MathsProgressPanel;
    [SerializeField] GameObject m_UrduProgressPanel;


    [SerializeField] GameObject m_StudentInfoPanel;
    [SerializeField] ScrollManager m_ScollManager;

    [Header("Recommendation Stuff")]
    [SerializeField] GameObject m_RecommendationPanel;
    [SerializeField] TMP_InputField m_InputRecommendation;
    [SerializeField] TextMeshProUGUI m_RecommentationText;


    //List<Student> students;
    public static TeacherPortalManager instance;
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
        sendMessage.AddInts(UIManagerNew.Singleton.Teacher.GetStudentsID().ToArray());
        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    public void OnClickSelectRecommendationButton()
    {
        UIManagerNew.Singleton.isOnClickRecommendations = true;
        PloteStudentInfoPanel();
    }

    public void ShowStudentRecommendation(Student student)
    {
        selectedStudent = student;
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.StudentRecommendation);
        sendMessage.AddStudent(student);
        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    public void OnClickSendStudentRecommendation()
    {
        if(m_InputRecommendation.text != string.Empty)
        {
            Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.SendRecommendation);
            sendMessage.AddStudent(instance.selectedStudent);
            sendMessage.AddString(m_InputRecommendation.text);
            NetworkManager.Singleton.Client.Send(sendMessage);
        }
    }

    [MessageHandler((ushort)ClientToServer.SendRecommendation)]
    private static void ReceiveStudentRecommendation(Message message)
    {
        instance.m_RecommentationText.text = instance.m_InputRecommendation.text;
        instance.m_InputRecommendation.text = string.Empty;
    }

    #region Individual Progress

    public void OnClickIndividualProgress()
    {
        UIManagerNew.Singleton.isOnClickRecommendations = false;
        PloteStudentInfoPanel();
    }

    void PloteStudentInfoPanel()
    {
        m_StudentsIDPanel.SetActive(true);
        foreach (Student s in UIManagerNew.Singleton.students)
        {
            GameObject studentInfoPanel = m_ScollManager.AddToFront(m_StudentInfoPanel);
            studentInfoPanel.GetComponent<StudentUI>().SetData(s);
            studentInfoPanels.Add(studentInfoPanel);
        }
    }

    public void OnClickReturnToSelectTask_FromIndividualStudentSelector()
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

    #endregion


    #region Overall Progress

    public void OnClickViewClassProgress_SubjectSelector()
    {
        m_OverallProgressPanelSelector.SetActive(true);
    }

    public void OnClickCloseClassProgress_SubjectSelector()
    {
        m_OverallClassProgressSelector.SetActive(false);
    }

    public void OnClickVisualizeOverallEnglishProgress()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.OCP_English);

        List<int> ids = new List<int>();
        List<string> names = new List<string>();

        foreach (Student s in UIManagerNew.Singleton.students)
        {
            ids.Add(s.ID);
            names.Add(s.Name);
        }

        sendMessage.AddInts(ids.ToArray());
        sendMessage.AddStrings(names.ToArray());

        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    [MessageHandler((ushort)ClientToServer.OCP_English)]
    private static void ReceiveStudentsOverallDataEnglish(Message _message)
    {
        string progress = _message.GetString();
        OverallProgress overallProgress = instance.FromJsonToOverallProgress(progress);

        List<int> progresses = new List<int>();
        List<string> names = new List<string>();

        foreach (StudentProgress pros in overallProgress.studentProgresses)
        {
            progresses.Add(pros.progress);
            names.Add(pros.studentName);
        }
        instance.ViewOverallProgress(progresses, names, "English");
    }

    public void OnClickVisualizeOverallMathsProgress()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.OCP_Maths);

        List<int> ids = new List<int>();
        List<string> names = new List<string>();

        foreach (Student s in UIManagerNew.Singleton.students)
        {
            ids.Add(s.ID);
            names.Add(s.Name);
        }

        sendMessage.AddInts(ids.ToArray());
        sendMessage.AddStrings(names.ToArray());

        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    [MessageHandler((ushort)ClientToServer.OCP_Maths)]
    private static void ReceiveStudentsOverallDataMaths(Message _message)
    {
        string progress = _message.GetString();
        OverallProgress overallProgress = instance.FromJsonToOverallProgress(progress);
        List<int> progresses = new List<int>();
        List<string> names = new List<string>();
        foreach (StudentProgress pros in overallProgress.studentProgresses)
        {
            progresses.Add(pros.progress);
            names.Add(pros.studentName);
        }
        instance.ViewOverallProgress(progresses, names, "Maths");
    }

    public void OnClickVisualizeOverallUrduProgress()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.OCP_Urdu);

        List<int> ids = new List<int>();
        List<string> names = new List<string>();

        foreach (Student s in UIManagerNew.Singleton.students)
        {
            ids.Add(s.ID);
            names.Add(s.Name);
        }

        sendMessage.AddInts(ids.ToArray());
        sendMessage.AddStrings(names.ToArray());

        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    [MessageHandler((ushort)ClientToServer.OCP_Urdu)]
    private static void ReceiveStudentsOverallDataUrdu(Message _message)
    {
        string progress = _message.GetString();
        OverallProgress overallProgress = instance.FromJsonToOverallProgress(progress);
        List<int> progresses = new List<int>();
        List<string> names = new List<string>();
        foreach (StudentProgress pros in overallProgress.studentProgresses)
        {
            progresses.Add(pros.progress);
            names.Add(pros.studentName);
        }
        instance.ViewOverallProgress(progresses, names, "Urdu");
    }

    void ViewOverallProgress(List<int> _progresses, List<string> _names, string _subNmae)
    {
        m_OverallProgressPanel.SetActive(true);
        OverallProgressGraph graph = m_OverallProgressPanel.GetComponent<OverallProgressGraph>();
        graph.ShowGraph(_progresses, _names, -1, _subNmae);
    }


    OverallProgress FromJsonToOverallProgress(string _jsonData)
    {
        try
        {
            OverallProgress overallProgress = JsonUtility.FromJson<OverallProgress>(_jsonData);
            return overallProgress == null ? throw new InvalidDataException("Data is in invalid format") : overallProgress;
        }
        catch (Exception ex)
        {
            print("Json string is Corrupted!!!!!. Cause: " + ex.Message);
            return null;
        }
    }

    #endregion
}
