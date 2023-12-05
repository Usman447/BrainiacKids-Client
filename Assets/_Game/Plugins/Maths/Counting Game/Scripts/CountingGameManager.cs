using NetworkDataManager;
using Scene_Management;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Maths.Counting
{
    public class CountingGameManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_CountingText;
        [SerializeField] GameObject m_StarPrefab;
        [SerializeField] Transform m_StarsParent;
        [SerializeField] int m_NumberOfStarsSpawn = 5;

        WinDialog winDialog;

        int counter = 0;
        int Counter
        {
            get { return counter; }
            set
            {
                counter = value;
                m_CountingText.text = counter.ToString();
                if (AudioManager.Singleton != null)
                {
                    AudioManager.Singleton.Play(counter.ToString());
                }
            }
        }
        
        public static CountingGameManager Instance;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            winDialog = FindObjectOfType<WinDialog>();
            Instance = this;
            Counter = counter;
            SpawnStarsOnRandomPlaces();
        }

        private void Start()
        {
            PlayMusic("type1");
        }

        public void CheckTheStar(StarScript _star)
        {
            if(_star.isStar)
            {
                Counter++;
                Destroy(_star.gameObject);
                
                if(Counter == m_NumberOfStarsSpawn)
                {
                    winDialog.ShowResults();

                    PlayMusic("Won");
                    if (NetworkData.Instance != null)
                    {
                        if (NetworkData.Instance.Data.Maths.Counting.Stars < winDialog.Stars)
                        {
                            NetworkData.Instance.Data.Maths.Counting.Stars = winDialog.Stars;
                            NetworkData.Instance.SendStudentGameStatus();
                        }
                    }
                }
            }
        }

        void SpawnStarsOnRandomPlaces()
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            int width = (int)(screenSize.x - m_StarPrefab.GetComponent<RectTransform>().rect.width - 225);
            int height = (int)(screenSize.y - m_StarPrefab.GetComponent<RectTransform>().rect.height - 210);

            width /= 2;
            height /= 2;

            float zOffset = 0.1f;
            for (int i = 1; i <= m_NumberOfStarsSpawn; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-width, width), Random.Range(-height, height), zOffset);
                zOffset += 0.1f;

                GameObject newStarObj = Instantiate(m_StarPrefab, m_StarsParent);
                newStarObj.GetComponent<RectTransform>().localPosition = spawnPos;
                newStarObj.GetComponent<StarScript>().isStar = true;
            }
        }

        public void ResetGameScene()
        {
            SceneTransition.Singleton.LoadScene(SceneManager.GetActiveScene().buildIndex, "Button");
        }

        public void ReturnToMainMenu()
        {
            if (AudioManager.Singleton != null)
                AudioManager.Singleton.Stop("type1");

            PlayMusic("Game Sound");
            SceneTransition.Singleton.LoadScene("Main menu", "Back");
        }

        public void PlayMusic(string _musicName)
        {
            if (AudioManager.Singleton != null)
                AudioManager.Singleton.Play(_musicName);
        }
    }
}
    