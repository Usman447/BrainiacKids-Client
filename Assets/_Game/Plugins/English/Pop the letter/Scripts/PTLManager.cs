using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using NetworkDataManager;
using Scene_Management;

namespace English.PopTheLetter
{
    public class PTLManager : MonoBehaviour
    {
        [SerializeField] GameObject[] m_AlphabetPrefab;
        [SerializeField] Transform m_SpawningPosition;
        [SerializeField] Camera cam;
        [SerializeField] FixedPoint point;
        [SerializeField] int m_RespawnedLetterCount = 5;
        [SerializeField] GameObject m_ResultPanel;

        public int CorrectAlphabetsCount = 0;
        public int WrongAlphabetsCount = 0;
        public int MissingAlphabetsCount = 0;
        public string letter;

        public static PTLManager instance;

        public bool GameEnd { get; private set; }
        int spawningCount = 0;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            instance = this;

            spawningCount = 0;
            char randChar = (char)Random.Range(65, 91);
            letter = randChar.ToString();
            GameEnd = false;
        }

        private void Start()
        {
            CorrectAlphabetsCount = 0;
            WrongAlphabetsCount = 0;
            MissingAlphabetsCount = 0;

            StartCoroutine(ShowResultPanel(45));

            SpeakLetter();

            InvokeSpawnAlphabet(0.1f, 0.2f);
            InvokeSpawnAlphabet(0.2f, 0.3f);
            InvokeSpawnAlphabet(0.3f, 0.4f);
            InvokeSpawnAlphabet(0.4f, 0.5f);
        }


        IEnumerator ShowResultPanel(float t)
        {
            yield return new WaitForSeconds(t);
            GameEnd = true;
            m_ResultPanel.SetActive(true);
            var panel = m_ResultPanel.GetComponent<PTLResultPanel>();
            if(panel != null)
            {
                panel.ShowResult(CorrectAlphabetsCount, WrongAlphabetsCount,
                    MissingAlphabetsCount);

                PlayMusic("Won");

                if (NetworkData.Instance != null)
                {
                    if (NetworkData.Instance.Data.English.PopTheLetters.Right < CorrectAlphabetsCount ||
                        NetworkData.Instance.Data.English.PopTheLetters.Wrong > WrongAlphabetsCount ||
                        NetworkData.Instance.Data.English.PopTheLetters.Missing > MissingAlphabetsCount)
                    {
                        NetworkData.Instance.Data.English.PopTheLetters.Right = CorrectAlphabetsCount;
                        NetworkData.Instance.Data.English.PopTheLetters.Wrong = WrongAlphabetsCount;
                        NetworkData.Instance.Data.English.PopTheLetters.Missing = MissingAlphabetsCount;
                        NetworkData.Instance.SendStudentGameStatus();
                    }
                }
            }
        }

        void SpawnRandomAlphabet()
        {
            GameObject randObj = m_AlphabetPrefab[Random.Range(0, m_AlphabetPrefab.Length)];

            GameObject obj = Instantiate(randObj, m_SpawningPosition.position, m_SpawningPosition.rotation);
            float width = Random.Range(0, point.width - 2);
            width *= (Random.Range(0, 2) == 0 ? -1 : 1);

            // If spanwed Letter is same as the selected one then spawn one step forward
            string spawnedLetter = obj.gameObject.name[0].ToString();
            Vector3 spawnedPosition = new Vector3(width, obj.transform.position.y, 0);
            spawnedPosition.z = spawnedLetter == letter ? -1 : 0;
            obj.transform.position = spawnedPosition;

            if(spawnedLetter != letter)
            {
                spawningCount++;
            }
            else
            {
                spawningCount = 0;
            }

            if(spawningCount == m_RespawnedLetterCount)
            {
                SpawnAlphabet(letter);
            }

            if(!GameEnd)
                InvokeSpawnAlphabet(3f, 4.5f);
        }

        void SpawnAlphabet(string _letter)
        {
            char c = _letter[0];
            int alphaInt = (int)c;

            alphaInt -= 65;

            GameObject randObj = m_AlphabetPrefab[alphaInt];

            GameObject obj = Instantiate(randObj, m_SpawningPosition.position, m_SpawningPosition.rotation);
            float width = Random.Range(0, point.width - 2);
            width *= (Random.Range(0, 2) == 0 ? -1 : 1);

            // If spanwed Letter is same as the selected one then spawn one step forward
            string spawnedLetter = obj.gameObject.name[0].ToString();
            Vector3 spawnedPosition = new Vector3(width, obj.transform.position.y, 0);
            spawnedPosition.z = spawnedLetter == _letter ? -1 : 0;
            obj.transform.position = spawnedPosition;

            spawningCount = 0;
            InvokeSpawnAlphabet(3f, 4.5f);
        }

        void InvokeSpawnAlphabet(float min, float max)
        {
            Invoke(nameof(SpawnRandomAlphabet), Random.Range(min, max));
        }

        public void SpeakLetter()
        {
            if(AudioManager.Singleton != null)
                AudioManager.Singleton.PlayNewSound(letter, 3f);
        }

        public bool IsSameLetterClicked(string _clickedLetter)
        {
            return letter == _clickedLetter;
        }

        public void ReturnToMainMenu()
        {
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
    