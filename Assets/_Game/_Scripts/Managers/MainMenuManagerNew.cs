using NetworkDataManager;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerNew : MonoBehaviour
{
    [SerializeField] GameObject m_MainMenuPanel;
    [SerializeField] GameObject m_EnglishPanel;
    [SerializeField] GameObject m_MathsPanel;
    [SerializeField] GameObject m_UrduPanel;
    GameObject _selectedPanel;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _selectedPanel = null;

        if (NetworkData.Instance != null)
        {
            NetworkData.Instance.SendRquestForStudentData();
        }
    }

    public void OnClickOpenEnglishPanel()
    {
        _selectedPanel = m_EnglishPanel;
        SetActivePanel(true);
    }

    public void OnClickOpenMathsPanel()
    {
        _selectedPanel = m_MathsPanel;
        SetActivePanel(true);
    }

    public void OnClickOpenUrduPanel()
    {
        _selectedPanel = m_UrduPanel;
        SetActivePanel(true);
    }

    public void OnClickCloseSelectionPanel()
    {
        SetActivePanel(false);
        _selectedPanel = null;
    }

    public void OnClickChangeScene(string _sceneName)
    {
        if (AudioManager.Singleton != null)
        {
            AudioManager.Singleton.Stop("Game Sound");
        }
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

    void SetActivePanel(bool _active)
    {
        _selectedPanel.SetActive(_active);
        m_MainMenuPanel.SetActive(!_active);
    }

    public void PlayMusic(string _musicName)
    {
        if (AudioManager.Singleton != null)
            AudioManager.Singleton.Play(_musicName);
    }
}
