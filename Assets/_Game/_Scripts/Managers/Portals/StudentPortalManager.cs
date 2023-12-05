using Network;
using NetworkDataManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentPortalManager : MonoBehaviour
{
    [SerializeField] GameObject m_SubjectSelectionPanel;


    public void ShowStudentRecommendation()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.StudentRecommendation);
        sendMessage.AddStudent(NetworkData.Instance.Student);
        NetworkManager.Singleton.Client.Send(sendMessage);
    }



    // ///////////////  Individual English Panel ////////////////// //
    public void SendEnglishIndividualProgressRequest()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ISD_English);
        sendMessage.AddInt(NetworkData.Instance.Student.ID);
        sendMessage.AddString(NetworkData.Instance.Student.Name);
        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    // ///////////////  Individual Maths Panel ////////////////// //


    public void SendMathsIndividualProgressRequest()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ISD_Maths);
        sendMessage.AddInt(NetworkData.Instance.Student.ID);
        sendMessage.AddString(NetworkData.Instance.Student.Name);
        NetworkManager.Singleton.Client.Send(sendMessage);
    }


    // ///////////////  Individual Urdu Panel ////////////////// //

    public void SendUrduIndividualProgressRequest()
    {
        Message sendMessage = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ISD_Urdu);
        sendMessage.AddInt(NetworkData.Instance.Student.ID);
        sendMessage.AddString(NetworkData.Instance.Student.Name);
        NetworkManager.Singleton.Client.Send(sendMessage);
    }

    public void OnClickSelectionPanel()
    {
        m_SubjectSelectionPanel.SetActive(true);
    }

    public void OnClickCloseSelectionPanel()
    {
        m_SubjectSelectionPanel.SetActive(false);
    }
}
