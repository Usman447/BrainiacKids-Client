using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Maths.AdditionSubtraction
{
    public class Equation
    {
        public int num1, num2, result;
        public string sign;

        public Equation(int num1, string sign, int num2, int result)
        {
            this.num1 = num1;
            this.num2 = num2;
            this.sign = sign;
            this.result = result;
        }

        public void Display()
        {
            Debug.Log($"{num1} {sign} {num2} = {result}");
        }
    }

    public class ASEquation : MonoBehaviour
    {
        [SerializeField] Image Num1, Num2;
        [SerializeField] Image Result;
        [SerializeField] TextMeshProUGUI Sign;
        [SerializeField] Sprite[] m_NumbersSprites;
        [SerializeField] Button m_ShowResultButton;

        Image disappearedNumber;
        [HideInInspector] public int disappeardResult, enteredNumber;

        public static ASEquation Instance;
        public Equation equation { get; private set; }

        private void Awake()
        {
            Instance = this;
            equation = GetRandomEquation(Random.Range(2, 11));

            SetEquationValues(equation);
            DisableButton();
        }

        public void UploadResult(int _enteredNumber)
        {
            disappearedNumber.enabled = true;
            disappearedNumber.sprite = GetNumberSprite(_enteredNumber);
            enteredNumber = _enteredNumber;
            //m_ShowResultButton.gameObject.SetActive(true);

            FindObjectOfType<ASManager>().OnClickCheckResult();
        }

        public bool IsCorrectMatched()
        {
            if (enteredNumber == disappeardResult)
            {
                return true;
            }
            return false;
        }

        public void DisableButton()
        {
            m_ShowResultButton.gameObject.SetActive(false);
        }

        void SetEquationValues(Equation _e)
        {
            _e.Display();

            Num1.sprite = GetNumberSprite(_e.num1);
            Sign.text = _e.sign;
            Num2.sprite = GetNumberSprite(_e.num2);
            Result.sprite = GetNumberSprite(_e.result);

            WhatToDisappear();
        }

        Equation GetRandomEquation(int result)
        {
            List<Equation> equations = new List<Equation>();

            for (int a = 1; a <= 10; a++)
            {
                for (int b = 1; b <= 10; b++)
                {
                    if (a + b == result)
                    {
                        equations.Add(new(a, "+", b, result));
                    }
                    if (a - b == result)
                    {
                        equations.Add(new(a, "-", b, result));
                    }
                }
            }

            return equations[Random.Range(0, equations.Count)];
        }

        public Sprite GetNumberSprite(int _num)
        {
            return m_NumbersSprites[_num - 1];
        }

        void WhatToDisappear()
        {
            int rand = Random.Range(1, 4);

            switch (rand)
            {
                case 1:
                    disappeardResult = equation.num1;
                    disappearedNumber = Num1;
                    break;
                case 2:
                    disappeardResult = equation.num2;
                    disappearedNumber = Num2;
                    break;
                case 3:
                    disappeardResult = equation.result;
                    disappearedNumber = Result;
                    break;
            }

            disappearedNumber.sprite = null;
            disappearedNumber.enabled = false;
        }
    }
}
    