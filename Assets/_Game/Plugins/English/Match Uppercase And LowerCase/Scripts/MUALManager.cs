using NetworkDataManager;
using Scene_Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace English.MatchingAlphabets
{
    // A -> 65
    // a -> 97

    public class MUALManager : MonoBehaviour
    {
        [SerializeField] GameObject m_ProtectedPanel;

        [Header("Grid Variables")]
        [SerializeField] GameObject m_UppercaseLetterPrefab;
        [SerializeField] GameObject m_LowercaseLetterPrefab;
        [SerializeField] Transform m_UppercaseTrans;
        [SerializeField] Transform m_LowercaseTrans;
        [SerializeField] int m_AlphabetsCount;

        [Header("Centered Alphabets")]
        [SerializeField] Alphabet m_CenteredUpper;
        [SerializeField] Alphabet m_CenteredLower;

        // private variables
        public static MUALManager instance;
        List<char> _spawnUpperLetters;

        Alphabet upperLetter;
        Alphabet lowerLetter;
        int currentAlphabetsCount;
        WinDialog winDialog;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            instance = this;
            winDialog = FindObjectOfType<WinDialog>();

            _spawnUpperLetters = new List<char>();
            currentAlphabetsCount = m_AlphabetsCount;
        }

        private void Start()
        {
            m_CenteredLower.gameObject.SetActive(false);
            m_CenteredUpper.gameObject.SetActive(false);

            PlayMusic("type1");

            SpawnUppercaseLetters();
            SpawnLowercaseLetters();
        }

        void SpawnUppercaseLetters()
        {
            for (int i = 0; i < m_AlphabetsCount; ++i)
            {
                GameObject uppercaseHolder = Instantiate(m_UppercaseLetterPrefab, m_UppercaseTrans);

                int randAlphabet = Random.Range(0, 26);
                char letter = (char)(randAlphabet + 65);
                _spawnUpperLetters.Add(letter);

                Alphabet uLetter = uppercaseHolder.GetComponent<Alphabet>();
                if (uLetter != null)
                {
                    uLetter.SetValues(letter);
                }
            }
        }

        void SpawnLowercaseLetters()
        {
            for (int i = 0; i < m_AlphabetsCount; ++i)
            {
                GameObject lowercaseHolder = Instantiate(m_LowercaseLetterPrefab, m_LowercaseTrans);
                char randAlphabet = GetRandomLowercaseLetter();

                Alphabet lLetter = lowercaseHolder.GetComponent<Alphabet>();
                if (lLetter != null)
                {
                    lLetter.SetValues(randAlphabet);
                }
            }
        }

        char GetRandomLowercaseLetter()
        {
            char randLetter = _spawnUpperLetters[Random.Range(0, _spawnUpperLetters.Count)];
            _spawnUpperLetters.Remove(randLetter);
            int randLetterNumber = (int)randLetter;
            randLetterNumber += 32;
            return (char)randLetterNumber;
        }

        public void SelectUpperAlphabet(Alphabet _letter)
        {
            if(upperLetter != null)
            {
                upperLetter.SetActiveStatus(true);
            }

            upperLetter = _letter;
            upperLetter.SetActiveStatus(false);

            m_CenteredUpper.gameObject.SetActive(true);
            m_CenteredUpper.SetValues(_letter._alphabet);
            StartCoroutine(CompareAlphabets(upperLetter, lowerLetter));
        }

        public void DeselectUpperAlphabet(Alphabet _letter)
        {
            upperLetter.SetActiveStatus(true);
            upperLetter = null;

            _letter.SetValues(' ');
            m_CenteredUpper.gameObject.SetActive(false);
        }

        public void SelectLowerAlphabet(Alphabet _letter)
        {
            if(lowerLetter != null)
            {
                lowerLetter.SetActiveStatus(true);
            }

            lowerLetter = _letter;
            lowerLetter.SetActiveStatus(false);

            m_CenteredLower.gameObject.SetActive(true);
            m_CenteredLower.SetValues(_letter._alphabet);
            StartCoroutine(CompareAlphabets(upperLetter, lowerLetter));
        }

        public void DeselectLowerAlphabet(Alphabet _letter)
        {
            lowerLetter.SetActiveStatus(true);
            lowerLetter = null;

            _letter.SetValues(' ');
            m_CenteredLower.gameObject.SetActive(false);
        }

        IEnumerator CompareAlphabets(Alphabet _upper, Alphabet _lower)
        {
            if(IsMatched(_upper, _lower))
            {
                upperLetter = null;
                lowerLetter = null;

                m_ProtectedPanel.SetActive(true);
                yield return new WaitForSeconds(0.75f);
                m_ProtectedPanel.SetActive(false);

                m_CenteredUpper.gameObject.SetActive(false);
                m_CenteredLower.gameObject.SetActive(false);
                
                currentAlphabetsCount--;

                if(currentAlphabetsCount <= 0)
                {
                    winDialog.ShowResults();
                    PlayMusic("Won");
                    if (NetworkData.Instance != null)
                    {
                        if (NetworkData.Instance.Data.English.MatchLetters.Stars < winDialog.Stars)
                        {
                            NetworkData.Instance.Data.English.MatchLetters.Stars = winDialog.Stars;
                            NetworkData.Instance.SendStudentGameStatus();
                        }
                    }
                }
            }
            else
            {
                print("Not Mateched");
            }
        }

        bool IsMatched(Alphabet _upper, Alphabet _lower)
        {
            if (_upper == null || _lower == null)
            {
                return false;
            }

            int _upperInt = (int)_upper._alphabet;
            int _lowerInt = (int)_lower._alphabet;

            _upperInt += 32;

            if (_upperInt == _lowerInt)
                return true;
            else
                return false;
        }

        public void ResetScene()
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
    