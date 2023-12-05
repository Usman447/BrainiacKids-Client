using Network;
using NetworkDataManager;
using Scene_Management;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerNew : MonoBehaviour
{
    public enum Selector
    {
        Teacher,
        Parent,
        Student
    }

    private static UIManagerNew _singleton;
    public static UIManagerNew Singleton
    {
        get => _singleton;
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Destroy(value);
                Debug.Log($"{nameof(UIManagerNew)} instance already exists, destroying duplicate");
            }
        }
    }

    [Header("Login Selector")]
    [SerializeField] GameObject m_LoginSelectorPanel;


    [Header("Login")]
    [SerializeField] GameObject m_LoginPanel;

    [SerializeField] TMP_InputField m_IpAddress;
    [SerializeField] TMP_InputField m_Username;
    [SerializeField] TMP_InputField m_Password;
    [SerializeField] GameObject m_InvalidLoginText;

    [Header("Teacher Panel")]
    [SerializeField] GameObject m_TeacherPortal;

    [Header("Parent Panel")]
    [SerializeField] GameObject m_ParentPortal;

    [Header("Student Panel")]
    [SerializeField] GameObject m_StudentPortal;

    [Header("Visualize Panels")]
    [SerializeField] GameObject m_EnglishProgressPanel;
    [SerializeField] GameObject m_MathsProgressPanel;
    [SerializeField] GameObject m_UrduProgressPanel;


    [Header("Recommendation Stuff")]
    [SerializeField] GameObject m_RecommendationPanelTeacher;
    [SerializeField] TextMeshProUGUI m_RecommentationTextTeacher;

    [SerializeField] GameObject m_RecommendationPanelParent;
    [SerializeField] TextMeshProUGUI m_RecommentationTextParent;

    [SerializeField] GameObject m_RecommendationPanelStudent;
    [SerializeField] TextMeshProUGUI m_RecommentationTextStudent;

    Selector selector;
    public Teacher Teacher { get; private set; } = null;
    public Parent Parent { get; private set; } = null;
    public bool isTeacherLogin { get; private set; }
    public List<Student> students { get; private set; }
    public bool isOnClickRecommendations { get; set; }
    int recommendationCounter;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Singleton = this;
        Teacher = null;
        Parent = null;
        students = new List<Student>();
        recommendationCounter = -1;
    }

    private void Start()
    {
        NetworkManager.Singleton.Client.ConnectionFailed += ClientConnectionFailed;
        NetworkManager.Singleton.Client.Disconnected += ClientDisconnected;

        PlayMusic("Game Sound");
    }

    #region Event Functions

    private void ClientConnectionFailed(object sender, ConnectionFailedEventArgs e)
    {
        if (SceneTransition.Singleton != null)
        {
            if (students != null)
                students.Clear();
            SceneTransition.Singleton.LoadScene("Login Scene");
        }
    }

    private void ClientDisconnected(object sender, DisconnectedEventArgs e)
    {
        if (SceneTransition.Singleton != null)
        {
            if (students != null)
                students.Clear();

            SceneTransition.Singleton.LoadScene("Login Scene");
        }
    }

    #endregion


    public void OnClickLoginButton()
    {
        if (selector == Selector.Teacher)
        {
            SendTeacherLoginRequest();
        }
        else if (selector == Selector.Parent)
        {
            SendParentLoginRequest();
        }
        else if (selector == Selector.Student)
        {
            SendStudentLoginRequest();
        }
        else
        {
            Debug.LogWarning("No selector is selected !!!!!");
        }
    }

    void SendTeacherLoginRequest(string _username, string _password)
    {
        if (NetworkManager.Singleton.Connect("127.0.0.1"))
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.LoginTeacher);
            message.AddLogin(_username, _password);
            NetworkManager.Singleton.Client.Send(message);
        }
    }

    void SendTeacherLoginRequest()
    {
        if (NetworkManager.Singleton.Connect(m_IpAddress.text))
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.LoginTeacher);
            message.AddLogin(m_Username.text, m_Password.text);
            NetworkManager.Singleton.Client.Send(message);
        }
        m_Username.text = string.Empty;
        m_Password.text = string.Empty;
    }

    void SendParentLoginRequest(string _username, string _password)
    {
        if (NetworkManager.Singleton.Connect("127.0.0.1"))
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.LoginParent);
            message.AddLogin(_username, _password);
            NetworkManager.Singleton.Client.Send(message);
        }
    }

    void SendParentLoginRequest()
    {
        if (NetworkManager.Singleton.Connect(m_IpAddress.text))
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.LoginParent);
            message.AddLogin(m_Username.text, m_Password.text);
            NetworkManager.Singleton.Client.Send(message);
        }
        m_Username.text = string.Empty;
        m_Password.text = string.Empty;
    }

    void SendStudentLoginRequest(string _username, string _password)
    {
        if (NetworkManager.Singleton.Connect("127.0.0.1"))
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.LoginStudent);
            message.AddLogin(_username, _password);
            NetworkManager.Singleton.Client.Send(message);
        }
    }

    void SendStudentLoginRequest()
    {
        if (NetworkManager.Singleton.Connect(m_IpAddress.text))
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.LoginStudent);
            message.AddLogin(m_Username.text, m_Password.text);
            NetworkManager.Singleton.Client.Send(message);
        }
        m_Username.text = string.Empty;
        m_Password.text = string.Empty;
    }


    [MessageHandler((ushort)ClientToServer.StudentRecommendation)]
    private static void ReceiveStudentRecommendationResult(Message message)
    {
        string receivedRec = message.GetString();
        print(receivedRec);

        if (Singleton.recommendationCounter == 1)
        {
            Singleton.m_RecommendationPanelTeacher.SetActive(true);
            if (receivedRec.Length != 0)
            {
                Singleton.m_RecommentationTextTeacher.text = receivedRec;
            }
            else
            {
                Singleton.m_RecommentationTextTeacher.text = "No Recommendation Given yet";
            }
        }
        else if (Singleton.recommendationCounter == 2)
        {
            Singleton.m_RecommendationPanelParent.SetActive(true);
            if (receivedRec.Length != 0)
            {
                Singleton.m_RecommentationTextParent.text = receivedRec;
            }
            else
            {
                Singleton.m_RecommentationTextParent.text = "No Recommendation Given yet";
            }
        }
        else if (Singleton.recommendationCounter == 3)
        {
            Singleton.m_RecommendationPanelStudent.SetActive(true);
            if (receivedRec.Length != 0)
            {
                Singleton.m_RecommentationTextStudent.text = receivedRec;
            }
            else
            {
                Singleton.m_RecommentationTextStudent.text = "No Recommendation Given yet";
            }
        }
    }


    // Teacher On Success of Login
    [MessageHandler((ushort)ClientToServer.LoginTeacher)]
    private static void ReceiveTeacherLoginRequest(Message message)
    {
        Singleton.m_LoginPanel.SetActive(false);
        Singleton.m_InvalidLoginText.SetActive(false);

        Singleton.SavePreviousIPAddress();

        Singleton.Teacher = message.GetTeacher();
        Singleton.isTeacherLogin = true;
        Singleton.recommendationCounter = 1;
        Singleton.m_TeacherPortal.SetActive(true);
    }

    [MessageHandler((ushort)ClientToServer.LoginParent)]
    private static void ReceiveParentLoginRequest(Message message)
    {
        Singleton.m_LoginPanel.SetActive(false);
        Singleton.m_InvalidLoginText.SetActive(false);

        Singleton.SavePreviousIPAddress();

        Singleton.Parent = message.GetParent();
        Singleton.isTeacherLogin = false;
        Singleton.recommendationCounter = 2;
        Singleton.m_ParentPortal.SetActive(true);
    }

    // Student On Success of Login
    [MessageHandler((ushort)ClientToServer.LoginStudent)]
    private static void ReceiveStudentLoginRequest(Message message)
    {
        Student student = message.GetStudent();
        NetworkData.Instance.Student = new Student(student.ID, student.Name);
        Singleton.SavePreviousIPAddress();
        Singleton.recommendationCounter = 3;
        Singleton.m_StudentPortal.SetActive(true);
    }

    // ///////////////  Student IDs ////////////////// //

    [MessageHandler((ushort)ClientToServer.StudentIDAndName)]
    private static void ReceiveStudentsIDs(Message message)
    {
        Student student = message.GetStudent();
        Singleton.students.Add(student);
    }


    // ///////////////  Individual English Panel ////////////////// //

    [MessageHandler((ushort)ClientToServer.ISD_English)]
    private static void ReceiveStudentDataEnglish(Message _message)
    {
        string data = _message.GetString();
        if (data != string.Empty)
        {
            Singleton.m_EnglishProgressPanel.SetActive(true);
            Singleton.ShowEnglishGraphPanel(data);
        }
    }

    void ShowEnglishGraphPanel(string _data)
    {
        EnglishData englishData = NetworkData.Instance.FromJsonToEnglish(_data);
        m_EnglishProgressPanel.GetComponent<EnglishPanel>().VisualizeGraphs(englishData);
    }

    // ///////////////  Individual Maths Panel ////////////////// //

    [MessageHandler((ushort)ClientToServer.ISD_Maths)]
    private static void ReceiveStudentDataMaths(Message _message)
    {
        string data = _message.GetString();
        if (data != string.Empty)
        {
            Singleton.m_MathsProgressPanel.SetActive(true);
            Singleton.ShowMathsGraphPanel(data);
        }
    }

    void ShowMathsGraphPanel(string _data)
    {
        MathsData mathsData = NetworkData.Instance.FromJsonToMaths(_data);
        m_MathsProgressPanel.GetComponent<MathsPanel>().VisualizeGraphs(mathsData);
    }

    // ///////////////  Individual Urdu Panel ////////////////// //


    [MessageHandler((ushort)ClientToServer.ISD_Urdu)]
    private static void ReceiveStudentDataUrdu(Message _message)
    {
        string data = _message.GetString();
        if (data != string.Empty)
        {
            Singleton.m_UrduProgressPanel.SetActive(true);
            Singleton.ShowUrduGraphPanel(data);
        }
    }

    void ShowUrduGraphPanel(string _data)
    {
        UrduData urduData = NetworkData.Instance.FromJsonToUrdu(_data);
        m_UrduProgressPanel.GetComponent<UrduPanel>().VisualizeGraphs(urduData);
    }

    void SavePreviousIPAddress()
    {
        PlayerPrefs.SetString("PreviousIPAddress", m_IpAddress.text);
    }

    [MessageHandler((ushort)ClientToServer.Invalid)]
    private static void ReceiveInvalidUser(Message message)
    {
        Debug.Log(message.GetString());
        Singleton.m_InvalidLoginText.SetActive(true);

        Singleton.PlayMusic("Wrong Password");
    }

    public void OnClickSelectTeacher()
    {
        selector = Selector.Teacher;
        SetActiveSelectorPanel(false);
    }

    public void OnClickSelectParent()
    {
        selector = Selector.Parent;
        SetActiveSelectorPanel(false);
    }

    public void OnClickSelectStudent()
    {
        selector = Selector.Student;
        SetActiveSelectorPanel(false);
    }

    public void OnClickReturnToLoginSelectorPanel()
    {
        SetActiveSelectorPanel(true);
    }

    public void OnClickPlayGame()
    {
        SceneManager.LoadScene("Main menu");
    }
    
    public void OnClickDisconnectClient()
    {
        NetworkManager.Singleton.Client.Disconnect();
    }

    /// <summary>
    /// Active the Selector panel if true
    /// </summary>
    /// <param name="active"></param>
    void SetActiveSelectorPanel(bool active)
    {
        m_LoginSelectorPanel.SetActive(active);
        m_LoginPanel.SetActive(!active);
    }

    public void PlayMusic(string _musicName)
    {
        if (AudioManager.Singleton != null)
            AudioManager.Singleton.Play(_musicName);
    }
}
