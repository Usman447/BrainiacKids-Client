using Network;
using Network.Utils;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
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
                Destroy(value.gameObject);
                Debug.LogWarning($"{nameof(NetworkManager)} instance already exists, destroying duplicate");
            }
        }
    }

    [SerializeField] string m_ConnectedIpAddress;
    [SerializeField] ushort m_Port;

    public Client Client { get; set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this.gameObject);

        Singleton = this;
        NetworkLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client(new Network.Transports.Tcp.TcpClient());
    }

    public bool Connect(string _ipAddress)
    {
        if (isIpFormat(_ipAddress))
        {
            Client.Connect($"{_ipAddress}:{m_Port}");
            return true;
        }
        else
        {
            Debug.Log("Invalid IP Address Format");
        }

        return false;
    }

    public bool Connect()
    {
        if (isIpFormat(m_ConnectedIpAddress))
        {
            Client.Connect($"{m_ConnectedIpAddress}:{m_Port}");
            return true;
        }
        return false;
    }

    bool isIpFormat(string _ip)
    {
        string[] values = _ip.Split('.');
        if (values.Length == 4)
            return true;
        return false;
    }

    private void Update()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

}
