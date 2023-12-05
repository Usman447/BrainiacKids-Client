using NetworkDataManager;
using Scene_Management;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Maths.NumberSorting
{
    public class NumberSortingManager : MonoBehaviour
    {
        [SerializeField] GameObject m_BallonPrefab;
        [SerializeField] Transform m_BallonsParent;
        [SerializeField] Transform m_SortedBallonPanel;
        [SerializeField] int m_NumberOfBallons = 5;
        [SerializeField] int bottomHeightOffset;

        [SerializeField] Sprite[] m_BallonSprites;
        [SerializeField] bool isAssending = true;

        [SerializeField] TextMeshProUGUI m_GameTypeText;

        public static NumberSortingManager Instance;

        int counting = -1;
        WinDialog winDialog;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            winDialog = FindObjectOfType<WinDialog>();
            Instance = this;

            SpawnBallonsOnRandomPlaces();
        }

        private void Start()
        {
            PlayMusic("type2");
        }

        public void CheckBallonNumber(NumberSortingBallon _ballon)
        {
            if(counting == -1)
            {
                Debug.LogError("Counting is -1");
                return;
            }

            if(_ballon.BallonNumber() != counting)
            {
                return;
            }

            if (isAssending)
            {
                counting++;
            }
            else
            {
                counting--;
            }

            _ballon.transform.SetParent(m_SortedBallonPanel);
            GameCompleted();
        }

        void GameCompleted()
        {
            if (IsGameCompleted())
            {
                winDialog.ShowResults();

                PlayMusic("Won");

                if (NetworkData.Instance != null)
                {
                    if (NetworkData.Instance.Data.Maths.Sorting.Stars < winDialog.Stars)
                    {
                        NetworkData.Instance.Data.Maths.Sorting.Stars = winDialog.Stars;
                        NetworkData.Instance.SendStudentGameStatus();
                    }
                }
            }
        }

        public void ResetGame()
        {
            SceneTransition.Singleton.LoadScene(SceneManager.GetActiveScene().buildIndex, "Button");
        }

        bool IsGameCompleted()
        {
            if ((isAssending && counting == m_NumberOfBallons) ||
                (!isAssending && counting <= 0))
            {
                return true;
            }
            return false;
        }

        void SpawnBallonsOnRandomPlaces()
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            int width = (int)(screenSize.x - m_BallonPrefab.GetComponent<RectTransform>().rect.width);
            int height = (int)(screenSize.y - m_BallonPrefab.GetComponent<RectTransform>().rect.height);

            width /= 2;
            height /= 2;

            float zOffset = 0;

            if (isAssending)
            {
                zOffset = 0.1f * m_NumberOfBallons;
                counting = 1;
            }
            else
            {
                zOffset = 0.1f;
                counting = m_NumberOfBallons;
            }

            for (int i = 1; i <= m_NumberOfBallons; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-width, width), Random.Range(-height + bottomHeightOffset, height), zOffset);

                if (isAssending)
                {
                    zOffset -= 0.1f;
                    m_GameTypeText.text = "Ascending";
                }
                else
                {
                    zOffset += 0.1f;
                    m_GameTypeText.text = "Decending";
                }

                GameObject newBallon = Instantiate(m_BallonPrefab, m_BallonsParent);
                newBallon.GetComponent<RectTransform>().localPosition = spawnPos;

                newBallon.GetComponent<Image>().sprite = m_BallonSprites[Random.Range(0, m_BallonSprites.Length)];

                newBallon.GetComponent<NumberSortingBallon>().UpdateNumber(i);
            }
        }

        public void ReturnToMainMenu()
        {
            if (AudioManager.Singleton != null)
                AudioManager.Singleton.Stop("type2");

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
    