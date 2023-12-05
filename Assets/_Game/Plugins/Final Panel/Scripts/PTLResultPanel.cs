using UnityEngine;
using TMPro;

namespace English.PopTheLetter
{
    public class PTLResultPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CorrectText;
        [SerializeField] TextMeshProUGUI WrongText;
        [SerializeField] TextMeshProUGUI MissingText;

        public void ShowResult(int correct, int wrong, int missing)
        {
            CorrectText.text = correct.ToString();
            WrongText.text = wrong.ToString();
            MissingText.text = missing.ToString();
        }
    }
}
    