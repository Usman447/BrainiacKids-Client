using NetworkDataManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Network;
using Scene_Management;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject m_BlockingPanel;
    [SerializeField] AllowNewInstance allowNewInstanceSO;

    private void Awake()
    {
        m_BlockingPanel.SetActive(false);

        if (allowNewInstanceSO.isAllowedNewInstance)
        {
            print("Call Awake");
            if (NetworkData.Instance != null)
            {
                NetworkData.Instance.SendRquestForStudentData();
            }

            /*if (AudioManager.Singleton != null)
            {
                AudioManager.Singleton.Play("Game Sound");
            }*/

            allowNewInstanceSO.isAllowedNewInstance = false;
        }
    }

    public void ReturnToLoginPage()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Client.Disconnect();
        }

        if (SceneTransition.Singleton != null)
        {
            SceneTransition.Singleton.LoadScene("Login Scene", "Back");
        }
    }

    public void OnClickChangeScene(string _sceneName)
    {
        /*if (AudioManager.Singleton != null)
        {
            AudioManager.Singleton.Play("Button");
        }*/

        m_BlockingPanel.SetActive(true);
        StartCoroutine(LoadAsyncScene(_sceneName));
    }

    IEnumerator LoadAsyncScene(string _sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
