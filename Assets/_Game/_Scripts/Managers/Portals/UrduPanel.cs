using MPUIKIT;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UrduPanel : MonoBehaviour
{

    [SerializeField] MPImage[] accuracyPieChart;

    [SerializeField] MPImage[] overallPieChart;
    [SerializeField] TextMeshProUGUI _averageText;

    [Header("Improvement Needed Stuff")]
    [SerializeField] TextMeshProUGUI m_UpperText;
    [SerializeField] TextMeshProUGUI m_LowerText;


    WindowGraph haroofETahaji, haroofLikhiye, haroofMilaye;
    List<string> gameNameList = new List<string>()
    {
        "Haroof-e-Tahaji",
        "Haroof Likhiye",
        "Haroof Milaye"
    };

    private void Awake()
    {
        haroofETahaji = transform.Find("HaroofETahaji").GetComponent<WindowGraph>();
        haroofLikhiye = transform.Find("HaroofLikhiye").GetComponent<WindowGraph>();
        haroofMilaye = transform.Find("HaroofMilaye").GetComponent<WindowGraph>();
    }


    public void VisualizeGraphs(UrduData urduData)
    {
        SetupHaroofETahajiGraph(urduData.HaroofETahaji);
        SetupHaroofLikhiyeGraph(urduData.HaroofLikhiye);
        SetupHaroofMilayeGraph(urduData.HaroofMilaye);

        SetupAccuracyPieChart(urduData);
        SetupOverallPirChart(urduData);
    }

    void SetupHaroofETahajiGraph(StarData _stars)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _stars.Stars, Color.blue));
        haroofETahaji.ShowGraph(list, 3);
    }

    void SetupHaroofLikhiyeGraph(List<TypeNameData> _likhiye)
    {
        float sum = 0;
        foreach (TypeNameData nameStars in _likhiye)
        {
            sum += (float)nameStars.Stars;
        }
        sum = (int)(sum / 78) * 3;

        if (sum == 0)
            sum = 1;

        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", (int)sum, Color.blue));
        haroofLikhiye.ShowGraph(list, 3);
    }

    void SetupHaroofMilayeGraph(StarData _stars)
    {
        List<GraphListData> list = new List<GraphListData>();
        list.Add(new GraphListData(" ", _stars.Stars, Color.blue));
        haroofMilaye.ShowGraph(list, 3);
    }

    void SetupAccuracyPieChart(UrduData urduData)
    {
        float sum = 0;
        foreach (TypeNameData nameStars in urduData.HaroofLikhiye)
        {
            sum += (float)nameStars.Stars;
        }
        sum = (int)(sum / 78) * 3;
        if (sum == 0)
            sum = 1;

        Calculator calculator = new Calculator();
        List<float> val = new List<float>
        {
            (float)urduData.HaroofETahaji.Stars / 3,
            (float)sum / 3,
            (float) urduData.HaroofMilaye.Stars / 3,
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

    void SetupOverallPirChart(UrduData urduData)
    {
        Calculator calculator = new Calculator();
        float average = calculator.Average(urduData);
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
