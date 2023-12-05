using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace English.MatchingAlphabets
{
    public class Alphabet : MonoBehaviour
    {
        TextMeshProUGUI _alphabetText;
        public char _alphabet { get; private set; }

        private void Awake()
        {
            _alphabetText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SelectUpperLetter()
        {
            MUALManager.instance.SelectUpperAlphabet(this);
        }

        public void DeselectUpperLetter()
        {
            MUALManager.instance.DeselectUpperAlphabet(this);
        }

        public void SelectLowerLetter()
        {
            MUALManager.instance.SelectLowerAlphabet(this);
        }

        public void DeselectLowerLetter()
        {
            MUALManager.instance.DeselectLowerAlphabet(this);
        }

        public void SetValues(char _alpha)
        {
            _alphabet = _alpha;
            _alphabetText.text = _alphabet.ToString();
        }

        public void SetActiveStatus(bool _status)
        {
            _alphabetText.gameObject.SetActive(_status);
            GetComponent<MPImage>().enabled = _status;
            GetComponent<Button>().interactable = _status;
        }
    }
}
