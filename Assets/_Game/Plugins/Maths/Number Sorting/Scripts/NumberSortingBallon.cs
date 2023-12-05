using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maths.NumberSorting
{
    public class NumberSortingBallon : MonoBehaviour
    {
        TextMeshProUGUI numberText { get; set; }
        Button button;
        int ballonNumber;

        private void Awake()
        {
            numberText = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponent<Button>();

            button.onClick.AddListener(() => NumberSortingManager.Instance.CheckBallonNumber(this));
        }

        public void UpdateNumber(int _number)
        {
            ballonNumber = _number;
            numberText.text = _number.ToString();
        }

        public int BallonNumber()
        {
            return ballonNumber;
        }
    }
}
    