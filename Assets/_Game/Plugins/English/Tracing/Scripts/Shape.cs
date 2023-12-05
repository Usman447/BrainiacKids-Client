using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace English.Tracing
{
    public class Shape : MonoBehaviour
    {
        /// <summary>
        /// The paths of the shape.
        /// </summary>
        public List<Path> paths;

        //0 - 10 seconds : 3 stars
        [Range(0, 500)] public int threeStarsTimePeriod = 10;

        //11 -20 seconds : 2 stars
        //more than 20 seconds : one star
        [Range(0, 500)] public int twoStarsTimePeriod = 20;

        /// <summary>
        /// Whether the shape is completed or not.
        /// </summary>
        [HideInInspector]
        public bool completed;

        /// <summary>
        /// Whether to enable the priority order or not.
        /// </summary>
        [HideInInspector]
        public bool enablePriorityOrder = true;

        private void Awake()
        {
            paths = GetComponentsInChildren<Path>().ToList();
        }

        void Start()
        {
            enablePriorityOrder = true;
            if (paths.Count != 0)
            {
                Invoke(nameof(EnableTracingHand), 0.2f);
                ShowPathNumbers(0);
            }
        }

        public void ShowPathNumbers(int index)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (i != index)
                {
                    paths[i].SetNumbersStatus(false);
                }
                else
                {
                    paths[i].SetNumbersStatus(true);
                }
            }
        }

        public int GetCurrentPathIndex()
        {
            int index = -1;
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i].completed)
                {
                    continue;
                }

                bool isCurrentPath = true;
                for (int j = 0; j < i; j++)
                {
                    if (!paths[j].completed)
                    {
                        isCurrentPath = false;
                        break;
                    }
                }

                if (isCurrentPath)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public bool IsCurrentPath(Path path)
        {
            bool isCurrentPath = false;

            if (!enablePriorityOrder)
            {
                return true;
            }

            if (path == null)
            {
                return isCurrentPath;
            }

            isCurrentPath = true;
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i].GetInstanceID() == path.GetInstanceID())
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (!paths[j].completed)
                        {
                            isCurrentPath = false;
                            break;
                        }
                    }
                    break;
                }
            }

            return isCurrentPath;
        }

        public void EnableTracingHand()
        {
            int currentPathIndex = GetCurrentPathIndex();
            if (currentPathIndex == -1)
            {
                Debug.LogWarning("NUll Path");
                return;
            }
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(name);
                animator.SetTrigger(paths[currentPathIndex].name.Replace("Path", name.Split('-')[0]));
            }
        }

        /// <summary>
        /// Disable the tracing hand.
        /// </summary>
        public void DisableTracingHand()
        {
            int currentPathIndex = GetCurrentPathIndex();
            if (currentPathIndex == -1)
            {
                Debug.LogWarning("NUll Path");
                return;
            }

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool(name, false);
                animator.SetBool(paths[currentPathIndex].name.Replace("Path", name.Split('-')[0]), false);
            }
        }
    }
}
    