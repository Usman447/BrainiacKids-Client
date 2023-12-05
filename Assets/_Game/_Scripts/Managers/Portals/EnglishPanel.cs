using MPUIKIT;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnglishPanel : MonoBehaviour
{
    [SerializeField]
    MPImage[] accuracyPieChart;

    [SerializeField]
    MPImage[] overallPieChart;
    [SerializeField] TextMeshProUGUI _averageText;

    [Header("Improvement Needed Stuff")]
    [SerializeField] TextMeshProUGUI m_UpperText;
    [SerializeField] TextMeshProUGUI m_LowerText;


    WindowGraph tracing, guessTheLetter, matchLetters, popTheLetters;
    List<string> gameNameList = new List<string>()
    {
        "Tracing Alphabets",
        "Guess the Letters",
        "Match Letters",
        "Pop the Letters"
    };

    private void Awake()
    {
        tracing = transform.Find("Tracing").GetComponent<WindowGraph>();
        guessTheLetter = transform.Find("Guess the letter").GetComponent<WindowGraph>();
        matchLetters = transform.Find("Match letters").GetComponent<WindowGraph>();
        popTheLetters = transform.Find("Pop the letters").GetComponent<WindowGraph>();
    }

    public void VisualizeGraphs(EnglishData _englishData)
    {
        SetupTracingGraph(_englishData.Tracking);
        SetupGuessTheLetter(_englishData.GuessLetters);
        SetupMatchLetters(_englishData.MatchLetters);
        SetupPopTheLetters(_englishData.PopTheLetters);
        SetupAccuracyPieChart(_englishData);
        SetupOverallPirChart(_englishData);
    }

    void SetupTracingGraph(List<TypeNameData> _tracing)
    {
        float sum = 0;
        foreach (TypeNameData nameStars in _tracing)
        {
            sum += (float)nameStars.Stars;
        }
        sum = (int)(sum / 78) * 3;

        if (sum == 0)
            sum = 1;

        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", (int)sum, Color.blue));
        tracing.ShowGraph(list, 3);
    }

    void SetupGuessTheLetter(StarData _stars)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _stars.Stars, Color.blue));
        guessTheLetter.ShowGraph(list, 3);
    }

    void SetupMatchLetters(StarData _stars)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _stars.Stars, Color.blue));
        matchLetters.ShowGraph(list, 3);
    }

    void SetupPopTheLetters(RightWrongMissingData _data)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _data.Right, Color.blue));
        list.Add(new GraphListData(" ", _data.Wrong, Color.cyan)); 
        list.Add(new GraphListData(" ", _data.Missing, Color.grey));

        int max;
        if (_data.Right > _data.Wrong && _data.Right > _data.Missing)
            max = _data.Right;
        else if (_data.Wrong > _data.Right && _data.Wrong > _data.Missing)
            max = _data.Wrong;
        else
            max = _data.Missing;

        popTheLetters.ShowGraph(list, max);
    }

    void SetupAccuracyPieChart(EnglishData _englishData)
    {
        float sum = 0;
        foreach (TypeNameData nameStars in _englishData.Tracking)
        {
            sum += (float)nameStars.Stars;
        }
        sum = (int)(sum / 78) * 3;
        if (sum == 0)
            sum = 1;

        Calculator calculator = new Calculator();
        List<float> val = new List<float>
        {
            (float)sum / 3,
            (float)_englishData.GuessLetters.Stars / 3,
            (float)_englishData.MatchLetters.Stars / 3,
            (float)calculator.GetRightWrongMissingDataStars(_englishData.PopTheLetters) / 3
        };

        float totalValues = 0;
        for (int i = 0; i < accuracyPieChart.Length; i++)
        {
            totalValues += calculator.FindPercentage(val, i);
            accuracyPieChart[i].fillAmount = totalValues;
        }


        float smallest1 = float.MaxValue;
        float smallest2 = float.MaxValue;

        int index1 = 0, index2 = 0;

        int ii = 0;
        foreach (float number in val)
        {
            if (number < smallest1)
            {
                smallest2 = smallest1;
                smallest1 = number;
                index1 = ii;
            }
            else if (number < smallest2)
            {
                smallest2 = number;
                index2 = ii;
            }
            ii++;
        }

        m_UpperText.text = gameNameList[index1];
        m_LowerText.text = gameNameList[index2];
    }

    void SetupOverallPirChart(EnglishData _englishData)
    {
        Calculator calculator = new Calculator();
        float average = calculator.Average(_englishData);
        overallPieChart[0].fillAmount = 1;
        average = (float)(average / 100);
        overallPieChart[1].fillAmount = average;
        _averageText.text = $"{average * 100}%";
    }

    public void OnClickCloseButton()
    {
        this.gameObject.SetActive(false);
    }

}
