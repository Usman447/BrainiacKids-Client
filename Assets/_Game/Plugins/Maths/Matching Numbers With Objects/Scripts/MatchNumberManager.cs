using JetBrains.Annotations;
using NetworkDataManager;
using Scene_Management;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Maths.MatchingNumber
{
    public class MatchNumberManager : MonoBehaviour
    {
        [SerializeField] GameObject[] m_Fruits;
        [SerializeField] float m_LeftMargins, m_RightMargins;
        [SerializeField] float m_TopMargins, m_BottomMargins;
        [SerializeField] Transform m_FriutsParent;

        public static MatchNumberManager Instance;

        Camera cam;
        Vector2 screenSize;

        GameObject selectedObject;
        Vector3 offset;
        WinDialog winDialog;
        int _right, _wrong;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            winDialog = FindObjectOfType<WinDialog>();
            Instance = this;
            _right = 0;
            _wrong = 0;

            cam = Camera.main;
            CalculateCameraBounds();
        }

        private void Start()
        {
            PlayMusic("type1");
            SpawnFriuts();
        }

        private void Update()
        {
            HoldAndDrag();
        }

        public void CheckFriutsCounter(MatchNumberObject _fruit)
        {
            if(_fruit != null)
            {
                Destroy(_fruit.gameObject);
                _right++;
                Invoke(nameof(IsGameCompleted), 0.1f);
            }
        }

        public void MissMatchObject(MatchNumberObject _fruit)
        {
            _wrong++;
            print("Miss Matching " + _fruit.name);
        }

        void IsGameCompleted()
        {
            var fruits = GameObject.FindGameObjectsWithTag("MatchNumberObject");
            print(fruits.Length);
            if(fruits.Length <= 0)
            {
                winDialog.ShowResults();

                PlayMusic("Won");

                if (NetworkData.Instance != null)
                {
                    if (NetworkData.Instance.Data.Maths.Matching.Stars < winDialog.Stars ||
                        NetworkData.Instance.Data.Maths.Matching.Right < _right ||
                        NetworkData.Instance.Data.Maths.Matching.Wrong > _wrong)
                    {
                        NetworkData.Instance.Data.Maths.Matching.Stars = winDialog.Stars;
                        NetworkData.Instance.Data.Maths.Matching.Right = _right;
                        NetworkData.Instance.Data.Maths.Matching.Wrong = _wrong;
                        NetworkData.Instance.SendStudentGameStatus();
                    }
                }
            }
        }

        void HoldAndDrag()
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePos);
                if (targetObject && targetObject.CompareTag("MatchNumberObject"))
                {
                    selectedObject = targetObject.transform.gameObject;
                    offset = selectedObject.transform.position - mousePos;
                }
            }

            if (selectedObject)
            {
                selectedObject.transform.position = mousePos + offset;
            }

            if (Input.GetMouseButtonUp(0) && selectedObject)
            {
                selectedObject = null;
            }
        }

        void SpawnFriuts()
        {
            foreach (GameObject obj in m_Fruits)
            {
                Spawn(obj);
            }

            int moreCount = Random.Range(1, 5);
            for (int i = 1; i <= moreCount; i++)
            {
                GameObject obj = m_Fruits[Random.Range(0, m_Fruits.Length)];
                Spawn(obj);
            }
        }

        void Spawn(GameObject obj)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-screenSize.x + m_LeftMargins, screenSize.x + m_RightMargins),
                    Random.Range(-screenSize.y + m_BottomMargins, screenSize.y + m_TopMargins));

            GameObject spawnFruit = Instantiate(obj, spawnPosition, Quaternion.identity);
            spawnFruit.transform.SetParent(m_FriutsParent);
        }

        void CalculateCameraBounds()
        {
            screenSize = new Vector2(cam.aspect * cam.orthographicSize, cam.orthographicSize);
        }

        public void ResetGame()
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
    