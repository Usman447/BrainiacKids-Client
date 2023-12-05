using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maths.Counting
{
    public class StarScript : MonoBehaviour
    {
        public bool isStar;

        private void Awake()
        {
            Button button = GetComponent<Button>();

            button.onClick.AddListener(() => CheckStarStatus());
        }

        void CheckStarStatus()
        {
            CountingGameManager.Instance.CheckTheStar(this);
        }
    }
}
    