using NetworkDataManager;
using Scene_Management;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Maths.AdditionSubtraction
{
    public class ASManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_ResultText;

        List<int> _addedNumbers;
        WinDialog winDialog;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            winDialog = FindObjectOfType<WinDialog>();
            _addedNumbers = new List<int>();
        }

        private void Start()
        {
            PlayMusic("type2");

            m_ResultText.enabled = false;
            var buttonObj = FindObjectsOfType<ASNumberButton>();
            if (buttonObj != null)
            {
                int randPos = Random.Range(0, buttonObj.Length);
                buttonObj[randPos].Number = ASEquation.Instance.disappeardResult;
                _addedNumbers.Add(ASEquation.Instance.disappeardResult);

                for (int i = 0; i < buttonObj.Length; i++)
                {
                    if (i != randPos)
                    {
                        buttonObj[i].Number = GetRandomNumberExcept();
                    }
                }
            }
        }

        public void OnClickCheckResult()
        {
            m_ResultText.enabled = true;
            if (ASEquation.Instance.IsCorrectMatched())
            {
                m_ResultText.color = Color.green;
                m_ResultText.text = "Correct Answer";
                ASEquation.Instance.DisableButton();
                foreach (var aSButton in FindObjectsOfType<ASNumberButton>())
                {
                    aSButton.button.interactable = false;
                }

                winDialog.ShowResults();

                PlayMusic("Won");

                if (NetworkData.Instance != null)
                {
                    if (NetworkData.Instance.Data.Maths.AdditionSubtraction.Stars < winDialog.Stars)
                    {
                        NetworkData.Instance.Data.Maths.AdditionSubtraction.Stars = winDialog.Stars;
                        NetworkData.Instance.SendStudentGameStatus();
                    }
                }
            }
            else
            {
                m_ResultText.color = Color.red;
                m_ResultText.text = "Incorrect Answer";
            }
        }

        int GetRandomNumberExcept()
        {
            int num;
            do
            {
                num = Random.Range(1, 11);
            } while (IsNumberAlreadyThere(num));
            _addedNumbers.Add(num);
            PrintList();
            return num;
        }

        bool IsNumberAlreadyThere(int num)
        {
            foreach (int n in _addedNumbers)
            {
                if (n == num)
                    return true;
            }
            return false;
        }

        void PrintList()
        {
            string val = "";
            foreach (var n in _addedNumbers)
            {
                val+= n.ToString() + " ";
            }
            print(val);
        }

        public void ResetGameScene()
        {
            SceneTransition.Singleton.LoadScene(SceneManager.GetActiveScene().buildIndex, "Button");
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
    