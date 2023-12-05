using MPUIKIT;
using UnityEngine;

namespace Urdu.Haroof_e_Tahaji
{
    public class HTHaroof : MonoBehaviour
    {
        public Vector3 InitialPosition { get; set; }

        MPImage haroofImage;
        GameObject selectedObject;
        Vector3 mousePosition;
        Collider2D collider2d;
        Vector3 offset;
        bool isInHole;

        public void UpdateHaroofImage(Sprite _image)
        {
            haroofImage.sprite = _image;
        }

        private void Awake()
        {
            haroofImage = GetComponent<MPImage>();
            collider2d = GetComponent<Collider2D>();
        }

        private void Start()
        {
            isInHole = false;
        }

        private void Update()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            
                if(hit.collider != null &&
                    hit.collider == collider2d)
                {
                    selectedObject = hit.collider.gameObject;
                    offset = hit.transform.position - mousePosition;
                }
            }

            if (selectedObject != null)
            {
                selectedObject.transform.position = mousePosition + offset;
            }

            if(Input.GetMouseButtonUp(0) &&
                selectedObject != null)
            {
                if(isInHole)
                {
                    HTManager.Singleton.AddHaroofName(haroofImage);
                }

                selectedObject = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Drop Hole"))
            {
                isInHole = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Drop Hole"))
            {
                isInHole = false;
            }
        }

    }
}
    