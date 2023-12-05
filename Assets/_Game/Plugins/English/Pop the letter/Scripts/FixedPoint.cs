using UnityEngine;

namespace English.PopTheLetter
{
    [ExecuteAlways]
    public class FixedPoint : MonoBehaviour
    {
        [SerializeField] Camera cam;

        float height;
        public float width;

        private void Update()
        {
            if (cam == null)
                return;

            height = 2f * cam.orthographicSize;
            width = height * cam.aspect;
            width /= 2;

            float upperPoint = (height / 2);
            transform.localPosition = new Vector3(transform.localPosition.x, upperPoint, 0);
        }
    }
}
    