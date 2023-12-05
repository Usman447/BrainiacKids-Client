using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace English.Tracing
{
    public class Path : MonoBehaviour
    {
        #region Enums
        public enum ShapeType
        {
            Horizontal,
            Vertical
        }

        public enum FillMethod
        {
            Radial, // Curves
            Linear, // Straight Line
            Point  // Click
        }

        #endregion

        #region Veriables
        /// <summary>
        /// Whether to flip the direction of the path or not.
        /// </summary>
        public bool flip;

        /// <summary>
        /// Whether the path is completed or not.
        /// </summary>
        [HideInInspector]
        public bool completed;

        /// <summary>
        /// The fill method (Radial or Linear or Point).
        /// </summary>
        public FillMethod fillMethod;


        /// <summary>
        /// The type of the shape (Used for Vertical Fill method).
        /// </summary>
        public ShapeType type = ShapeType.Vertical;

        /// <summary>
        /// The angle offset in degree(For Linear Fill).
        /// </summary>
        public float offset = 90;

        /// <summary>
        /// The complete offset (The fill amount offset).
        /// </summary>
        public float completeOffset = 0.85f;

        /// <summary>
        /// The first number reference.
        /// </summary>
        public Transform firstNumber;

        /// <summary>
        /// The second number reference.
        /// </summary>
        public Transform secondNumber;

        /// <summary>
        /// Whether to enable the quarter's restriction on the current angle or not.
        /// </summary>
        public bool quarterRestriction = true;

        /// <summary>
        /// The offset of the current radial angle(For Radial Fill) .
        /// </summary>
        public float radialAngleOffset = 0;

        /// <summary>
        /// The shape reference.
        /// </summary>
        public Shape shape;

        /// <summary>
        /// Whether to run the auto fill or not.
        /// </summary>
        bool autoFill;
        #endregion


        private void Awake()
        {
            shape = GetComponentInParent<Shape>();
        }


        public void AutoFill()
        {
            StartCoroutine(AutoFillCoroutine());
        }


        public void SetNumbersStatus(bool status)
        {
            Color tempColor = Color.white;
            List<Transform> numbers = Utils.FindChildrenByTag(transform.Find("Numbers"), "Number");
            foreach (Transform number in numbers)
            {
                if (number == null)
                    continue;

                if (status)
                {
                    EnableStartCollider();
                    number.GetComponent<Animator>().SetBool("Select", true);
                    tempColor.a = 1;
                }
                else
                {
                    if (shape == null)
                    {
                        Debug.Log("Shape is null"); return;
                    }

                    if (shape.enablePriorityOrder || completed)
                    {
                        DisableStartCollider();
                    }

                    if (shape.enablePriorityOrder)
                    {
                        number.GetComponent<Animator>().SetBool("Select", false);
                        tempColor.a = 0.3f;
                    }
                }

                number.GetComponent<Image>().color = tempColor;
            }
        }

        public void SetNumbersVisibility(bool visible)
        {
            List<Transform> numbers = Utils.FindChildrenByTag(transform.Find("Numbers").transform, "Number");
            foreach (Transform number in numbers)
            {
                if (number != null)
                    number.gameObject.SetActive(visible);
            }
        }

        public void EnableStartCollider()
        {
            transform.Find("Start").GetComponent<Collider2D>().enabled = true;
        }

        public void DisableStartCollider()
        {
            transform.Find("Start").GetComponent<Collider2D>().enabled = false;
        }

        public void ResetPath()
        {
            SetNumbersVisibility(true);
            completed = false;
            if (!shape.enablePriorityOrder)
            {
                SetNumbersStatus(true);
            }
            StartCoroutine(ReleaseCoroutine());
        }

        private IEnumerator AutoFillCoroutine()
        {
            Image image = Utils.FindChildByTag(transform, "Fill").GetComponent<Image>();
            while (image.fillAmount < 1)
            {
                image.fillAmount += 0.02f;
                yield return new WaitForSeconds(0.001f);
            }
        }

        private IEnumerator ReleaseCoroutine()
        {
            Image image = Utils.FindChildByTag(transform, "Fill").GetComponent<Image>();
            while (image.fillAmount > 0)
            {
                image.fillAmount -= 0.02f;
                yield return new WaitForSeconds(0.005f);
            }
        }

    }
}
    