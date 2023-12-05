using UnityEngine;
using System;
using System.IO;
using Network;

namespace NetworkDataManager
{

    public enum ClientToServer : ushort
    {
        // Login
        LoginTeacher = 1,
        LoginParent = 2,
        LoginStudent = 3,
        Invalid = 4,

        // Student In Game Data
        StudentInGameDataRequest = 5,
        StudentInGameDataSave = 6,

        // Individual Student Data Request
        ISD_English = 7,
        ISD_Urdu = 8,
        ISD_Maths = 9,

        // Overall Class Progress
        OCP_English = 10,
        OCP_Urdu = 11,
        OCP_Maths = 12,

        // Recommendation
        StudentRecommendation = 13,

        // Student Name and ID to get data
        StudentIDAndName = 14,

        // Send Recommendation
        SendRecommendation = 15,
    }

    public class NetworkData : MonoBehaviour
    {
        private static NetworkData _instance;
        public static NetworkData Instance
        {
            get => _instance;
            set
            {
                if (_instance == null)
                {
                    _instance = value;
                }
                else if (_instance != value)
                {
                    Destroy(value);
                    Debug.Log($"{nameof(NetworkData)} instance already exists, destroying duplicate");
                }
            }
        }

        public Student Student { get; set; }

        public Data Data { get; set; }

        private void Awake()
        {
            Instance = this;
            Student = null;
        }

        public string MakeJsonFormat(Data data)
        {
            return JsonUtility.ToJson(data);
        }


        public Data FromJsonToData(string _jsonFormatData)
        {
            try
            {
                Data data = JsonUtility.FromJson<Data>(_jsonFormatData);
                return data == null ? throw new InvalidDataException("Data is in invalid format") : data;
            }
            catch (Exception ex)
            {
                print("Json string is Corrupted!!!!!. Cause: " + ex.Message);
                return null;
            }
        }


        // ////////////////////// English Data /////////////////// //
        public string ToJson(EnglishData data)
        {
            return JsonUtility.ToJson(data);
        }

        public EnglishData FromJsonToEnglish(string _jsonFormatData)
        {
            try
            {
                EnglishData englishData = JsonUtility.FromJson<EnglishData>(_jsonFormatData);
                return englishData == null ? throw new InvalidDataException("Data is in invalid format") : englishData;
            }
            catch (Exception ex)
            {
                print("Json string is Corrupted!!!!!. Cause: " + ex.Message);
                return null;
            }
        }


        // ////////////////////// Maths Data /////////////////// //
        public string ToJson(MathsData data)
        {
            return JsonUtility.ToJson(data);
        }

        public MathsData FromJsonToMaths(string _jsonFormatData)
        {
            try
            {
                MathsData mathsData = JsonUtility.FromJson<MathsData>(_jsonFormatData);
                return mathsData == null ? throw new InvalidDataException("Data is in invalid format") : mathsData;
            }
            catch (Exception ex)
            {
                print("Json string is Corrupted!!!!!. Cause: " + ex.Message);
                return null;
            }
        }


        // ////////////////////// Urdu Data /////////////////// //
        public string ToJson(UrduData data)
        {
            return JsonUtility.ToJson(data);
        }

        public UrduData FromJsonToUrdu(string _jsonFormatData)
        {
            try
            {
                UrduData urduData = JsonUtility.FromJson<UrduData>(_jsonFormatData);
                return urduData == null ? throw new InvalidDataException("Data is in invalid format") : urduData;
            }
            catch (Exception ex)
            {
                print("Json string is Corrupted!!!!!. Cause: " + ex.Message);
                return null;
            }
        }



        // Data Sender
        public void SendStudentGameStatus()
        {
            if (NetworkManager.Singleton != null)
            {
                Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.StudentInGameDataSave);

                message.AddInt(Student.ID);
                message.AddString(Student.Name);
                message.AddString(MakeJsonFormat(Data));

                NetworkManager.Singleton.Client.Send(message);
            }
            else
                Debug.LogWarning("Null reference Network Manager");
        }

        [MessageHandler((ushort)ClientToServer.StudentInGameDataSave)]
        private static void ReceiveStudentInGameDataSaveAcknowledge(Message _message)
        {
            print(_message.GetString());
        }


        public void SendRquestForStudentData()
        {
            if (NetworkManager.Singleton != null)
            {
                Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.StudentInGameDataRequest);

                message.AddInt(Student.ID);
                message.AddString(Student.Name);

                NetworkManager.Singleton.Client.Send(message);
            }
        }


        [MessageHandler((ushort)ClientToServer.StudentInGameDataRequest)]
        private static void ReceiveStudentInGameDataResult(Message _message)
        {
            string receivedStr = _message.GetString();
            print(receivedStr);
            Instance.Data = Instance.FromJsonToData(receivedStr);
        }


    }
}
    