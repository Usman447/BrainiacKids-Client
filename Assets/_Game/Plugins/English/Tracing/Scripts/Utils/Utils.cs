using System.Collections.Generic;
using UnityEngine;
using System;

namespace English.Tracing
{
    public class Utils
    {
        /// <summary>
        /// Find the game objects of tag.
        /// </summary>
        /// <returns>The game objects of tag(Sorted by name).</returns>
        /// <param name="tag">Tag.</param>
        public static GameObject[] FindGameObjectsOfTag(string tag)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            Array.Sort(gameObjects, CompareGameObjects);
            return gameObjects;
        }

        /// <summary>
        /// Finds the direct child by tag.
        /// </summary>
        /// <returns>The child by tag.</returns>
        /// <param name="p">parent.</param>
        /// <param name="childTag">Child tag.</param>
        public static Transform FindChildByTag(Transform theParent, string childTag)
        {
            if (string.IsNullOrEmpty(childTag) || theParent == null)
            {
                return null;
            }

            foreach (Transform child in theParent)
            {
                if (child.tag == childTag)
                {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the direct children by tag.
        /// </summary>
        /// <returns>The children by tag.</returns>
        /// <param name="p">parent.</param>
        /// <param name="childrenTag">Child tag.</param>
        public static List<Transform> FindChildrenByTag(Transform theParent, string childTag)
        {
            List<Transform> children = new List<Transform>();

            if (string.IsNullOrEmpty(childTag) || theParent == null)
            {
                return children;
            }

            foreach (Transform child in theParent)
            {
                if (child.tag == childTag)
                {
                    children.Add(child);
                }
            }

            return children;
        }

        /// <summary>
        /// Covert RectTransform to screen space.
        /// </summary>
        /// <returns>The transform to screen space.</returns>
        /// <param name="transform">Transform.</param>
        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            return new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        }

        /// <summary>
        /// Compares the game objects.
        /// </summary>
        /// <returns>The game objects.</returns>
        /// <param name="gameObject1">Game object1.</param>
        /// <param name="gameObject2">Game object2.</param>
        private static int CompareGameObjects(GameObject g1, GameObject g2)
        {
            return g1.name.CompareTo(g2.name);
        }
    }
}
    