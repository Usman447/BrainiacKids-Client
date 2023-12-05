using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace English.GuessTheLetter
{
    public class ChoicePanel : MonoBehaviour
    {
        List<TextMeshProUGUI> _buttonText;
        ColorBlock _initialButtonColor;

        private void Awake()
        {
            _buttonText = GetComponentsInChildren<TextMeshProUGUI>().ToList();
            _initialButtonColor = transform.GetChild(0).GetComponent<Button>().colors;
        }

        public void SetButtonAlphabetsAsync(List<string> _objectsName, string _spawnedName)
        {
            List<char> _existedChar = new List<char>();

            char randName = _spawnedName[0];
            _existedChar.Add(randName);

            int added = Random.Range(0, _buttonText.Count);
            _buttonText[added].text = randName.ToString();

            for (int i = 0; i < _buttonText.Count; i++)
            {
                if (i == added)
                    continue;

                bool isAdded = false;
                do
                {
                    randName = _objectsName[Random.Range(0, _objectsName.Count)][0];

                    if (!_existedChar.Contains(randName))
                    {
                        _existedChar.Add(randName);
                        _buttonText[i].text = randName.ToString();
                        isAdded = true;
                    }

                } while (!isAdded);
            }
        }

        public void OnClickChoiceButton(int _index)
        {
            print(_index);

            string _chooseAlphabet = _buttonText[_index].text;

            Button _button = _buttonText[_index].GetComponentInParent<Button>();
            if(GLManager.instance.CompareNames(_chooseAlphabet))
            {
                // TODO: Add some animations 

                SetButtonColor(_button, Color.green);
            }
            else
            {
                SetButtonColor(_button, Color.red);
            }
        }

        public void ResetButtons()
        {
            foreach(Button button in GetComponentsInChildren<Button>())
            {
                button.interactable = true;
                button.colors = _initialButtonColor;
            }
        }

        void SetButtonColor(Button _pressedButton, Color color)
        {
            foreach(TextMeshProUGUI t in _buttonText)
            {
                Button _button = t.GetComponentInParent<Button>();
                if(color == Color.green)
                {
                    _button.interactable = false;
                }

                ColorBlock _bColor = _button.colors;
                _bColor.selectedColor = Color.white;
                _button.colors = _bColor;
            }

            ColorBlock _buttonColor = _pressedButton.colors;
            _buttonColor.selectedColor = color;
            
            if(color == Color.green)
            {
                color.a = 1;
                _buttonColor.disabledColor = color;
            }

            _pressedButton.colors = _buttonColor;
        }
    }
}
    