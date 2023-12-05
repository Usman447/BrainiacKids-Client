using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GraphListData
{
    public string xLabel;
    public int yLabel;
    public Color color;

    public GraphListData(string xLabel, int yLabel, Color color)
    {
        this.xLabel = xLabel;
        this.yLabel = yLabel;
        this.color = color;
    }
}

public class WindowGraph : MonoBehaviour
{
    //public List<GraphListData> GraphData;
    //public int ySize = 3;

    RectTransform graphContainer;
    RectTransform labelTemplateX;
    RectTransform labelTemplateY;
    RectTransform dashTemplateY;

    List<GameObject> gameObjectList;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashY").GetComponent <RectTransform>();
        gameObjectList = new List<GameObject>();

        //ShowGraph(GraphData, -1);
    }

    public void ShowGraph(List<GraphListData> labels, int maxYSize, int maxVisibleValueAmount = -1)
    {
        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = labels.Count;
        }

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        float difference = graphHeight / maxYSize;
        float num = 0;

        List<float> yPos = new List<float>();
        for (int i = 0; i <= maxYSize; i++)
        {
            yPos.Add(num);
            num += difference;
        }



        for (int i = 0; i <= maxYSize; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-7f, yPos[i]);
            labelY.GetComponent<TextMeshProUGUI>().text = (i).ToString();
            gameObjectList.Add(labelY.gameObject);


            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(14, (yPos[i] - (graphHeight / 2)));
            gameObjectList.Add(dashY.gameObject);
        }



        for (int i = 0, xIndex = 0; i < labels.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            GameObject barGameObject = CreateBar(new Vector2(xPosition, yPos[labels[i].yLabel]), 
                xSize * .9f, labels[i].color);

            gameObjectList.Add(barGameObject);

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -15f);
            labelX.GetComponent<TextMeshProUGUI>().text = labels[i].xLabel;
            gameObjectList.Add(labelX.gameObject);

            xIndex++;
        }
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth, Color _barColor)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.GetComponent<Image>().color = _barColor;
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(barWidth, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(.5f, 0f);
        return gameObject;
    }

    private void OnDisable()
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
    }
}
