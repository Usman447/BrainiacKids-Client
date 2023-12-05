using MPUIKIT;
using NetworkDataManager;
using Scene_Management;
using ScrollView;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Urdu.Haroof_e_Tahaji
{
    public class HTManager : MonoBehaviour
    {
        [SerializeField] GameObject[] ObjectPrefabs;
        [SerializeField] RectTransform SpawnPosition;
        [SerializeField] GameObject InvisiblePanel;

        public static HTManager Singleton;
        ScrollManager scrollManager;
        List<string> haroofSequence;
        List<string> haroofNames;
        Stack<GameObject> gameObjectStack;
        WinDialog winDialog;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            winDialog = FindObjectOfType<WinDialog>();
            Singleton = this;

            scrollManager = FindObjectOfType<ScrollManager>();

            haroofSequence = new List<string>();
            haroofNames = new List<string>();
            gameObjectStack = new Stack<GameObject>();

            GameObject obj = Instantiate(ObjectPrefabs[Random.Range(0, ObjectPrefabs.Length)], SpawnPosition);

            HTObject hTObject = obj.GetComponent<HTObject>();
            SpawnPosition.GetComponent<MPImage>().sprite = hTObject.Image;
            obj.GetComponent<RectTransform>().localPosition = -SpawnPosition.localPosition;

            InvisiblePanel.SetActive(false);
        }

        private void Start()
        {
            PlayMusic("urdugames");
        }

        bool IsHaroofSequenceMatched()
        {
            if(haroofSequence.Count != haroofNames.Count)
            {
                throw new Exception("Count Mismatching");
            }

            for(int i = 0; i < haroofSequence.Count; i++)
            {
                if (!haroofSequence[i].Equals(haroofNames[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public void AddHaroofName(MPImage _haroofName)
        {
            AddToStack(_haroofName.gameObject);
            haroofNames.Add(_haroofName.sprite.name);
            try
            {
                if (IsHaroofSequenceMatched())
                {
                    InvisiblePanel.SetActive(true);
                    winDialog.ShowResults();

                    PlayMusic("Won");

                    if (NetworkData.Instance != null)
                    {
                        if (NetworkData.Instance.Data.Urdu.HaroofETahaji.Stars < winDialog.Stars)
                        {
                            NetworkData.Instance.Data.Urdu.HaroofETahaji.Stars = winDialog.Stars;
                            NetworkData.Instance.SendStudentGameStatus();
                        }
                    }
                }
            }
            catch (Exception _e)
            {

            }
        }

        public void CreateHaroofsList(Sprite[] _haroofSprites)
        {
            foreach(Sprite haroof in _haroofSprites)
            {
                haroofSequence.Add(haroof.name);
            }
        }

        public void AddToStack(GameObject _obj)
        {
            _obj.GetComponent<HTHaroof>().enabled = false;
            scrollManager.AddToFront(_obj);
            gameObjectStack.Push(_obj);
            _obj.gameObject.SetActive(false);
        }

        public void RemoveFromStack()
        {
            if (gameObjectStack.TryPop(out GameObject obj))
            {
                haroofNames.Remove(obj.GetComponent<MPImage>().sprite.name);
                obj.SetActive(true);
                HTHaroof haroof = obj.GetComponent<HTHaroof>();
                haroof.enabled = true;
                obj.GetComponent<RectTransform>().localPosition = haroof.InitialPosition;
                scrollManager.RemoveFromFront();
            }
        }

        public void ResetScene()
        {
            SceneTransition.Singleton.LoadScene(SceneManager.GetActiveScene().buildIndex, "Button");
        }

        public void ReturnToMainMenu()
        {
            if (AudioManager.Singleton != null)
                AudioManager.Singleton.Stop("urdugames");

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
    