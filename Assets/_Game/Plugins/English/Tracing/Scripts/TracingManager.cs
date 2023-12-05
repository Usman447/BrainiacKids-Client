using NetworkDataManager;
using Scene_Management;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace English.Tracing
{
    public class TracingManager : MonoBehaviour
    {
        public bool isRunning = true;
        public int startingLetter;
        public Color CurrentColor;
        public Transform hand;
        public Camera cam;
        [SerializeField] Transform ParentTrans;
        [SerializeField] GameObject[] LettersPrefab;

        [SerializeField] GameObject MoveToNextButton;
        [SerializeField] GameObject MoveToHomeButton;

        Shape shape;
        Path path;
        Image pathFillImage;
        Vector3 clickPosition;
        Vector3 mousePosition;
        Vector2 direction;
        float angle, angleOffset, fillAmount;
        float clockwiseSign, targetQuarter;
        RaycastHit2D hit2d;
        WinDialog winDialog;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            winDialog = FindObjectOfType<WinDialog>();

            SpawnNewLetter();
        }

        private void Start()
        {
            PlayMusic("type2");
        }

        void SpawnNewLetter()
        {
            if(shape != null)
            {
                Destroy(shape.gameObject);
            }

            MoveToHomeButton.SetActive(false);
            MoveToNextButton.SetActive(false);

            GameObject spawnLetter = LettersPrefab[startingLetter];
            string letterName = spawnLetter.name;
            spawnLetter = Instantiate(spawnLetter, ParentTrans);
            spawnLetter.name = letterName;
            shape = spawnLetter.GetComponent<Shape>();
            winDialog.ResetResult();
        }

        private void Update()
        {
            if(shape == null)
            {
                Debug.LogError("Shape variable is null");
                return;
            }

            if(shape.completed)
            {
                return;
            }

            mousePosition = GetCurrentPlatformClickPosition(cam);

            if (Input.GetMouseButtonDown(0))    // Click
            {
                hit2d = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit2d.collider != null)
                {
                    if (hit2d.transform.tag == "Start")
                    {
                        OnStartHitCollider(hit2d);
                        shape.CancelInvoke();
                        shape.DisableTracingHand();
                        EnableHand();
                    }
                    else if (hit2d.transform.tag == "Collider")
                    {
                        shape.DisableTracingHand();
                        EnableHand();
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0)) // Stop Clicking
            {
                DisableHand();
                shape.Invoke(nameof(shape.EnableTracingHand), 0.25f);
                ResetPath();
            }


            if (!isRunning || path == null || pathFillImage == null)
            {
                return;
            }

            if (path.completed)
            {
                return;
            }

            DrawHand(mousePosition);

            // Reset the path if finger is away from the path
            hit2d = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit2d.collider == null)
            {
                ResetPath();
                return;
            }

            if (path.fillMethod == Path.FillMethod.Radial)
            {
                RadialFill();
            }
            else if (path.fillMethod == Path.FillMethod.Linear)
            {
                LinearFill();
            }
            else if (path.fillMethod == Path.FillMethod.Point)
            {
                PointFill();
            }

        }

        #region Filling Functions

        void RadialFill()
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            direction = clickPosition - path.transform.position;

            angleOffset = 0;
            clockwiseSign = (pathFillImage.fillClockwise ? 1 : -1);

            if (pathFillImage.fillOrigin == 0)
            {//Bottom
                angleOffset = 0;
            }
            else if (pathFillImage.fillOrigin == 1)
            {//Right
                angleOffset = clockwiseSign * 90;
            }
            else if (pathFillImage.fillOrigin == 2)
            {//Top
                angleOffset = -180;
            }
            else if (pathFillImage.fillOrigin == 3)
            {//left
                angleOffset = -clockwiseSign * 90;
            }

            angle = Mathf.Atan2(-clockwiseSign * direction.x, -direction.y) * Mathf.Rad2Deg + angleOffset;

            if (angle < 0)
                angle += 360;

            angle = Mathf.Clamp(angle, 0, 360);
            angle -= path.radialAngleOffset;

            if (path.quarterRestriction)
            {
                if (!(angle >= 0 && angle <= targetQuarter))
                {
                    pathFillImage.fillAmount = 0;
                    return;
                }

                if (angle >= targetQuarter / 2)
                {
                    targetQuarter += 90;
                }
                else if (angle < 45)
                {
                    targetQuarter = 90;
                }

                targetQuarter = Mathf.Clamp(targetQuarter, 90, 360);
            }

            fillAmount = Mathf.Abs(angle / 360.0f);
            pathFillImage.fillAmount = fillAmount;
            CheckPathComplete();
        }

        void LinearFill()
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = path.transform.eulerAngles;
            rotation.z -= path.offset;

            Rect rect = Utils.RectTransformToScreenSpace(path.GetComponent<RectTransform>());

            Vector3 pos1 = Vector3.zero, pos2 = Vector3.zero;

            if (path.type == Path.ShapeType.Horizontal)
            {
                pos1.x = path.transform.position.x - Mathf.Sin(rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;
                pos1.y = path.transform.position.y - Mathf.Cos(rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;

                pos2.x = path.transform.position.x + Mathf.Sin(rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;
                pos2.y = path.transform.position.y + Mathf.Cos(rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;
            }
            else
            {

                pos1.x = path.transform.position.x - Mathf.Cos(rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;
                pos1.y = path.transform.position.y - Mathf.Sin(rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;

                pos2.x = path.transform.position.x + Mathf.Cos(rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;
                pos2.y = path.transform.position.y + Mathf.Sin(rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;
            }

            pos1.z = path.transform.position.z;
            pos2.z = path.transform.position.z;

            if (path.flip)
            {
                Vector3 temp = pos2;
                pos2 = pos1;
                pos1 = temp;
            }

            clickPosition.x = Mathf.Clamp(clickPosition.x, Mathf.Min(pos1.x, pos2.x), Mathf.Max(pos1.x, pos2.x));
            clickPosition.y = Mathf.Clamp(clickPosition.y, Mathf.Min(pos1.y, pos2.y), Mathf.Max(pos1.y, pos2.y));
            fillAmount = Vector2.Distance(clickPosition, pos1) / Vector2.Distance(pos1, pos2);
            pathFillImage.fillAmount = fillAmount;
            CheckPathComplete();
        }

        void PointFill()
        {
            pathFillImage.fillAmount = 1;
            CheckPathComplete();
        }

        #endregion


        public void OnClickMoveToNextShape()
        {
            /*if (AudioManager.Singleton != null)
            {
                AudioManager.Singleton.Play("Button");
            }*/

            startingLetter++;
            SpawnNewLetter();
        }

        void CheckPathComplete()
        {
            if (fillAmount >= path.completeOffset)
            {
                path.completed = true;
                path.AutoFill();
                path.SetNumbersVisibility(false);
                ReleasePath();
                if (CheckShapeComplete())
                {
                    shape.completed = true;
                    OnShapeComplete();
                }

                shape.ShowPathNumbers(shape.GetCurrentPathIndex());

                hit2d = Physics2D.Raycast(GetCurrentPlatformClickPosition(Camera.main), Vector2.zero);
                if (hit2d.collider != null)
                {
                    if (hit2d.transform.tag == "Start")
                    {
                        if (shape.IsCurrentPath(hit2d.transform.GetComponentInParent<Path>()))
                        {
                            OnStartHitCollider(hit2d);
                        }
                    }
                }
            }
        }

        bool CheckShapeComplete()
        {
            bool shapeCompleted = true;
            Path[] paths = shape.GetComponentsInChildren<Path>();
            foreach (Path path in paths)
            {
                if (!path.completed)
                {
                    shapeCompleted = false;
                    break;
                }
            }
            return shapeCompleted;
        }

        void OnShapeComplete()
        {
            DisableHand();
            Animator shapeAnimator = shape.GetComponent<Animator>();
            shapeAnimator.SetBool(shape.name, false);
            shapeAnimator.SetTrigger("Completed");

            winDialog.ShowResults();
            Debug.Log($"Shape {shape.name} is Completed");

            PlayMusic("Won");

            if (NetworkData.Instance != null)
            {
                if (FindObjectOfType<UrduDumyObject>() != null) // Urdu Game
                {
                    int dataIndex = TryGetDataIndex(NetworkData.Instance.Data.Urdu.HaroofLikhiye, shape.name);

                    if (dataIndex == -1)
                    {
                        TypeNameData data = new TypeNameData(shape.name, winDialog.Stars);
                        NetworkData.Instance.Data.Urdu.HaroofLikhiye.Add(data);
                    }
                    else
                    {
                        if (NetworkData.Instance.Data.Urdu.HaroofLikhiye[dataIndex].Stars < winDialog.Stars)
                        {
                            NetworkData.Instance.Data.Urdu.HaroofLikhiye[dataIndex].Stars = winDialog.Stars;
                        }
                    }
                }
                else
                {
                    int dataIndex = TryGetDataIndex(NetworkData.Instance.Data.English.Tracking, shape.name);

                    if (dataIndex == -1)
                    {
                        TypeNameData data = new TypeNameData(shape.name, winDialog.Stars);
                        NetworkData.Instance.Data.English.Tracking.Add(data);
                    }
                    else
                    {
                        if (NetworkData.Instance.Data.English.Tracking[dataIndex].Stars < winDialog.Stars)
                        {
                            NetworkData.Instance.Data.English.Tracking[dataIndex].Stars = winDialog.Stars;
                        }
                    }
                }

                NetworkData.Instance.SendStudentGameStatus();
            }
            else
            {
                Debug.LogWarning("Network Data Instance is Missing");
            }

            if (startingLetter == LettersPrefab.Length - 1)
            {
                MoveToHomeButton.SetActive(true);
            }
            else
            {
                MoveToNextButton.SetActive(true);
            }
        }

        int TryGetDataIndex(List<TypeNameData> _data, string _letterName)
        {
            for(int i=0; i<_data.Count; i++)
            {
                if (_data[i].Name == _letterName)
                {
                    return i;
                }
            }
            return -1;
        }

        private void OnStartHitCollider(RaycastHit2D hit2d)
        {
            path = hit2d.transform.GetComponentInParent<Path>();

            pathFillImage = Utils.FindChildByTag(path.transform, "Fill").GetComponent<Image>();

            if (path.completed || !shape.IsCurrentPath(path)) 
            {
                ReleasePath();
            }
            else
            {
                path.StopAllCoroutines();
                Utils.FindChildByTag(path.transform, "Fill").GetComponent<Image>().color = CurrentColor;
            }

            if (path != null)
            {
                if (!path.shape.enablePriorityOrder)
                {
                    shape = path.shape;
                }
            }
        }

        void ReleasePath()
        {
            path = null;
            pathFillImage = null;
        }

        void DrawHand(Vector3 clickPosition)
        {
            if (hand == null)
            {
                return;
            }

            hand.transform.position = clickPosition;
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

        public void EnableHand()
        {
            hand.GetComponent<SpriteRenderer>().enabled = true;
        }

        public void DisableHand()
        {
            hand.GetComponent<SpriteRenderer>().enabled = false;
        }

        void ResetPath()
        {
            if (path != null)
                path.ResetPath();
            ReleasePath();
            ResetTargetQuarter();
        }

        void ResetTargetQuarter()
        {
            targetQuarter = 90;
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
    