using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NetworkDataManager;
using Scene_Management;

namespace English.GuessTheLetter
{
    public class GLManager : MonoBehaviour
    {
        [SerializeField] Transform m_ObjectPanelTrans;
        [SerializeField] GameObject[] VisualPrefabs;
        [SerializeField] ChoicePanel m_ChoicePanel;

        public static GLManager instance;

        string objName;
        List<string> _objNames;
        TextMeshProUGUI _spawnedObjText;
        GameObject activeObject;
        int playCount;
        WinDialog m_WinDialog;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            instance = this;
            m_WinDialog = FindObjectOfType<WinDialog>();

            _objNames = new List<string>();
            foreach (GameObject obj in VisualPrefabs)
            {
                _objNames.Add(obj.name);
            }

            playCount = 0;
        }

        private void Start()
        {
            ResetScene();
            PlayMusic("type1");
        }

        void ResetScene()
        {
            if(activeObject != null)
            {
                Destroy(activeObject);
            }

            SpawnScene();
            m_ChoicePanel.ResetButtons();
            playCount++;
        }

        void SpawnScene()
        {
            GameObject randObj = VisualPrefabs[Random.Range(0, VisualPrefabs.Length)];
            activeObject = Instantiate(randObj, m_ObjectPanelTrans);
            objName = randObj.name;
            string blankName = $"_{randObj.name.Remove(0, 1)}";
            _spawnedObjText = activeObject.GetComponentInChildren<TextMeshProUGUI>();
            _spawnedObjText.text = blankName;

            m_ChoicePanel.SetButtonAlphabetsAsync(_objNames, objName);
        }


        public bool CompareNames(string _chooseAlphabet)
        {
            if (objName[0].ToString().Equals(_chooseAlphabet))
            {
                print("Matched");
                _spawnedObjText.text = objName;

                if (playCount == 5)
                {
                    m_WinDialog.ShowResults();
                    PlayMusic("Won");
                    if(NetworkData.Instance != null)
                    {
                        if (NetworkData.Instance.Data.English.GuessLetters.Stars < m_WinDialog.Stars)
                        {
                            NetworkData.Instance.Data.English.GuessLetters.Stars = m_WinDialog.Stars;
                            NetworkData.Instance.SendStudentGameStatus();
                        }
                    }
                }
                else
                {
                    Invoke(nameof(ResetScene), 2);
                }

                return true;
            }
            else
            {
                print("Wrong Choice");
                return false;
            }
        }

        public void ResetGameScene()
        {
            SceneTransition.Singleton.LoadScene(SceneManager.GetActiveScene().buildIndex, "Button");
        }

        public void ReturnToMainMenu()
        {
            StopMusic("type1");
            PlayMusic("Game Sound");
            SceneTransition.Singleton.LoadScene("Main menu", "Back");
        }

        public void PlayMusic(string _musicName)
        {
            if (AudioManager.Singleton != null)
                AudioManager.Singleton.Play(_musicName);
        }

        public void StopMusic(string _musicName)
        {
            if (AudioManager.Singleton != null)
                AudioManager.Singleton.Stop(_musicName);
        }
    }
}
    