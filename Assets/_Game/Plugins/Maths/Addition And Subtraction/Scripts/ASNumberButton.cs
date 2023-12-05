using UnityEngine;
using UnityEngine.UI;

namespace Maths.AdditionSubtraction
{
    public class ASNumberButton : MonoBehaviour
    {
        int number;
        public int Number
        {
            get => number;
            set
            {
                number = value;
                GetComponent<Image>().sprite = ASEquation.Instance.GetNumberSprite(number);
            }
        }
        public Button button { get; private set; }

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(() =>
            ASEquation.Instance.UploadResult(Number));
        }
    }
}
    