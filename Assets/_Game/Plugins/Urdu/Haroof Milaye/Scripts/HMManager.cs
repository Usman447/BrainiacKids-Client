using MPUIKIT;
using NetworkDataManager;
using Scene_Management;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Urdu.HaroofMilaye
{
    public class HMManager : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] float DragOffset = 1;

        [SerializeField]
        Sprite[] haroofNames, haroofObjects;

        [SerializeField] Button CheckButton;
        [SerializeField] TextMeshProUGUI WonLostText;
        [SerializeField] GameObject InvisiblePanel;
        List<HMObject> imagePanels, namePanels;

        Vector3 mousePosition;
        Vector3 clickPosition;
        HMObject selectedObject;
        bool isDragging;
        bool isConnectedWithObject = false;
        WinDialog winDialog;

        int NOC;
        int NumberOfConnections
        {
            get => NOC;
            set
            {
                NOC = value;
                CheckButton.gameObject.SetActive(NumberOfConnections == namePanels.Count);
            }
        }
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            winDialog = FindObjectOfType<WinDialog>();

            imagePanels = new List<HMObject>();
            GameObject[] imagePanelsObj = GameObject.FindGameObjectsWithTag("Image Panel");
            foreach(GameObject panel in imagePanelsObj)
            {
                HMObject obj = panel.GetComponentInChildren<HMObject>();
                obj.isImage = true;
                imagePanels.Add(obj);
            }

            namePanels = new List<HMObject>();
            GameObject[] namePanelsObj = GameObject.FindGameObjectsWithTag("Name Panel");
            foreach (GameObject panel in namePanelsObj)
            {
                HMObject obj = panel.GetComponentInChildren<HMObject>();
                obj.isImage = false;
                namePanels.Add(obj);
            }

            isDragging = false;
            isConnectedWithObject = false;
            NumberOfConnections = 0;
            WonLostText.enabled = false;
            InvisiblePanel.SetActive(false);
        }

        private void Start()
        {
            PlayMusic("urdugames");
            InitializeImageAndNamePanels();
        }

        void InitializeImageAndNamePanels()
        {
            List<int> _shuffleNumebrs = GetRandomShuffleNumbers(5);
            List<int> _shuffleNumbersCopy = IntListCopy(_shuffleNumebrs);

            List<int> _nameShuffle = GetRandomShuffleNumbers(_shuffleNumebrs);
            List<int> _imageShuffle = GetRandomShuffleNumbers(_shuffleNumbersCopy);

            for (int i = 0; i < namePanels.Count; i++)
            {
                namePanels[i].GetComponentInParent<MPImage>().sprite = haroofNames[_nameShuffle[i]];
                namePanels[i].Name = haroofNames[_nameShuffle[i]].name;

                imagePanels[i].GetComponentInParent<MPImage>().sprite = haroofObjects[_imageShuffle[i]];
                imagePanels[i].Name = haroofObjects[_imageShuffle[i]].name;
            }
        }

        List<int> GetRandomShuffleNumbers(int maxNumber)
        {
            List<int> orderNumber = new List<int>();
            for (int i = 0; i < haroofNames.Length; i++)
            {
                orderNumber.Add(i);
            }

            List<int> randomOrder = new List<int>();
            for (int i = 0; i < maxNumber; i++)
            {
                int rand = Random.Range(0, orderNumber.Count);
                randomOrder.Add(orderNumber[rand]);

                orderNumber.RemoveAt(rand);
            }

            return randomOrder;
        }

        List<int> GetRandomShuffleNumbers(List<int> numbers)
        {
            int count = numbers.Count; 
            List<int> randomOrder = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int rand = Random.Range(0, numbers.Count);
                randomOrder.Add(numbers[rand]);
                numbers.RemoveAt(rand);
            }

            return randomOrder;
        }

        List<int> IntListCopy(List<int> _numbers)
        {
            List<int> copyList = new List<int>();

            foreach(int num in _numbers)
            {
                copyList.Add(num);
            }

            return copyList;
        }

        private void Update()
        {
            mousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(GetCurrentPlatformClickPosition(cam), Vector2.zero);

                if (hit.collider != null)
                {
                    HMObject hitObject = hit.transform.GetComponent<HMObject>();
                    if (hitObject != null /*&&
                        hitObject.ConnectedObject == null*/)
                    {
                        selectedObject = hitObject;
                        clickPosition = mousePosition;
                    }
                }
            }

            if (selectedObject != null &&
                Vector3.Distance(clickPosition, mousePosition) >= DragOffset)
            {
                if (selectedObject.ConnectedObject == null)
                {
                    selectedObject.DrawLine(mousePosition);
                    selectedObject.RotateObjectTowardsMousePosition(mousePosition);

                    // Attached with other panel logic
                    RaycastHit2D connectionHit = Physics2D.Raycast(GetCurrentPlatformClickPosition(cam), Vector2.zero);

                    if (connectionHit.collider != null &&
                    connectionHit.collider != selectedObject.Collider)
                    {
                        HMObject hitObject = connectionHit.transform.GetComponent<HMObject>();
                        if (hitObject != null &&
                            hitObject.ConnectedObject == null &&
                            selectedObject.isImage != hitObject.isImage)
                        {
                            selectedObject.ConnectedObject = hitObject;
                            hitObject.ConnectedObject = selectedObject;
                            hitObject.IsConnected = true;

                            selectedObject.RotateTowards2D(hitObject.ParentObject);
                            selectedObject.IsConnected = true;
                            selectedObject = null;

                            isConnectedWithObject = true;

                            NumberOfConnections++;
                        }
                    }
                }
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectedObject != null)
                {
                    if (!isDragging)
                    {
                        Onclick();
                    }

                    if (!isConnectedWithObject)
                        selectedObject.IsConnected = false;

                    selectedObject = null;
                }
                isDragging = false;
            }

        }

        public void OnClickCheckAllConnections()
        {
            bool isWon = true;
            foreach(HMObject panel in namePanels)
            {
                if(panel == null || panel.ConnectedObject == null) continue;

                if(!CheckConnection(panel, panel.ConnectedObject))
                {
                    isWon = false; 
                    break;
                }
            }

            WonLostText.enabled = true;
            InvisiblePanel.SetActive(true);

            if (isWon)
            {
                WonLostText.text = "You Won";
                WonLostText.color = Color.green;
                CheckButton.interactable = false;

                winDialog.ShowResults();

                PlayMusic("Won");

                if (NetworkData.Instance != null)
                {
                    if (NetworkData.Instance.Data.Urdu.HaroofMilaye.Stars < winDialog.Stars)
                    {
                        NetworkData.Instance.Data.Urdu.HaroofMilaye.Stars = winDialog.Stars;
                        NetworkData.Instance.SendStudentGameStatus();
                    }
                }
            }
            else
            {
                WonLostText.text = "Incorrect";
                WonLostText.color = Color.red;
            }
        }

        bool CheckConnection(HMObject obj1, HMObject obj2)
        {
            if(obj1 == null || obj2 == null) return false;

            if(obj1.Name == obj2.Name)
            {
                return true;
            }
            return false;
        }

        void Onclick()
        {
            if (selectedObject.ConnectedObject != null)
            {
                selectedObject.IsConnected = false;

                selectedObject.ConnectedObject.ConnectedObject = null;
                selectedObject.ConnectedObject.IsConnected = false;
                selectedObject.ConnectedObject = null;

                NumberOfConnections--;
            }
        }

        Vector3 GetCurrentPlatformClickPosition(Camera camera)
        {
            Vector3 clickPosition = Vector3.zero;

            if (Application.isMobilePlatform) // Mobile Platforms
            {
                if (Input.touchCount != 0)
                {
                    Touch touch = Input.GetTouch(0);
                    clickPosition = touch.position;
                }
            }
            else // Editor
            {
                clickPosition = Input.mousePosition;
            }

            clickPosition = camera.ScreenToWorldPoint(clickPosition);
            clickPosition.z = 0;
            return clickPosition;
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
    