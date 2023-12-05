using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MathsPanel : MonoBehaviour
{
    [SerializeField] MPImage[] accuracyPieChart;
    [SerializeField] MPImage[] overallPieChart;
    [SerializeField] TextMeshProUGUI _averageText;

    [Header("Improvement Needed Stuff")]
    [SerializeField] TextMeshProUGUI m_UpperText;
    [SerializeField] TextMeshProUGUI m_LowerText;


    WindowGraph counting, sorting, matching, additionSubtraction;
    List<string> gameNameList = new List<string>()
    {
        "Counting",
        "Sorting",
        "Matching",
        "Addition Subtraction"
    };

    private void Awake()
    {
        counting = transform.Find("Counting").GetComponent<WindowGraph>();
        sorting = transform.Find("Sorting").GetComponent<WindowGraph>();
        matching = transform.Find("Matching").GetComponent<WindowGraph>();
        additionSubtraction = transform.Find("Addition Subtraction").GetComponent<WindowGraph>();
    }

    public void VisualizeGraphs(MathsData mathsData)
    {
        SetupCountingGraph(mathsData.Counting);
        SetupSortingGraph(mathsData.Sorting);
        SetupMatchingGraph(mathsData.Matching);
        SetupAdditionSubtractionGraph(mathsData.AdditionSubtraction);

        SetupAccuracyPieChart(mathsData);
        SetupOverallPirChart(mathsData);
    }

    void SetupCountingGraph(StarData _stars)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _stars.Stars, Color.blue));
        counting.ShowGraph(list, 3);
    }

    void SetupSortingGraph(StarData _stars)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _stars.Stars, Color.blue));
        sorting.ShowGraph(list, 3);
    }

    void SetupMatchingGraph(RightWrongStarData _data)
    {
        Calculator calculator = new Calculator();
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", calculator.GetRightWrongStarDataStars(_data), Color.blue));
        matching.ShowGraph(list, 3);
    }

    void SetupAdditionSubtractionGraph(StarData _stars)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _stars.Stars, Color.blue));
        additionSubtraction.ShowGraph(list, 3);
    }

    void SetupAccuracyPieChart(MathsData mathsData)
    {
        Calculator calculator = new Calculator();
        List<float> val = new List<float>
        {
            (float) mathsData.Counting.Stars / 3,
            (float) mathsData.Sorting.Stars / 3,
            (float) calculator.GetRightWrongStarDataStars(mathsData.Matching) / 3,
            (float) mathsData.AdditionSubtraction.Stars / 3
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


    void SetupOverallPirChart(MathsData mathsData)
    {
        Calculator calculator = new Calculator();
        float average = calculator.Average(mathsData);

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
