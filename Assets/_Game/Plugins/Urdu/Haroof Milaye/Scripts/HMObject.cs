using TMPro;
using UnityEngine;

namespace Urdu.HaroofMilaye
{
    public class HMObject : MonoBehaviour
    {
        [SerializeField] float SmoothVelocity = 5;
        public string Name;

        HMObject connectedObject;
        public HMObject ConnectedObject
        {
            get => connectedObject;
            set
            {
                connectedObject = value;
            }
        }

        public bool IsConnected { get; set; }
        public Collider2D Collider { get; private set; }
        public RectTransform ParentObject;
        public bool isImage { get; set; }

        RectTransform rotatedBody { get; set; } = null;
        RectTransform rayObject = null;
        float currentVelocity;
        Camera cam;

        private void Awake()
        {
            rotatedBody = GetComponent<RectTransform>();
            rayObject = this.transform.Find("RayObject").GetComponent<RectTransform>();

            Collider = GetComponent<Collider2D>();
            cam = FindObjectOfType<Camera>();
        }

        void Update()
        {
            if (!IsConnected)
            {
                float smoothSpeed = Mathf.SmoothDamp(rayObject.sizeDelta.y, 0,
                    ref currentVelocity, SmoothVelocity * Time.deltaTime);
                RayFilling(smoothSpeed);
            }
        }

        public void DrawLine(Vector3 mousePosition)
        {
            Vector3 mousePos = ScreenToCanvasPoint(mousePosition);
            float distance = Vector2.Distance(ParentObject.localPosition, mousePos);
            RayFilling(distance);
        }

        void RayFilling(float _filler)
        {
            rayObject.sizeDelta = new Vector2(rayObject.sizeDelta.x, _filler);
        }

        public void RotateObjectTowardsMousePosition(Vector3 _towards)
        {
            _towards.z = rotatedBody.position.z - cam.transform.position.z;
            Vector3 targetPosition = cam.ScreenToWorldPoint(_towards);

            // Calculate the rotation angle
            Vector3 lookDirection = targetPosition - rotatedBody.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90;

            // Rotate the object towards the mouse position
            rotatedBody.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void RotateTowards2D(RectTransform towards)
        {
            float distance = Vector2.Distance(ParentObject.localPosition, towards.localPosition);
            RayFilling(distance);

            Vector3 lookDirection = (towards.localPosition - ParentObject.localPosition).normalized;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90;
            rotatedBody.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        Vector3 ScreenToCanvasPoint(Vector3 _object)
        {
            Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);

            Vector3 pos = Vector3.zero;

            if (_object.x > center.x)
            {
                pos.x = (center.x - _object.x) * -1;
            }
            else if (_object.x < center.x)
            {
                pos.x = -center.x + _object.x;
            }

            if (_object.y > center.y)
            {
                pos.y = (center.y - _object.y) * -1;
            }
            else if (_object.y < center.y)
            {
                pos.y = -center.y + _object.y;
            }
            return pos;
        }

    }
}
    