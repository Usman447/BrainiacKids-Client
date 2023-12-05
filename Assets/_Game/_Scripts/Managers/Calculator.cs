using System.Collections.Generic;

public class Calculator
{
    public int Average(EnglishData data)
    {
        float sum = 0;
        foreach (TypeNameData nameStars in data.Tracking)
        {
            sum += (float)nameStars.Stars;
        }

        sum = (int)(sum / 78) * 3;
        sum += (data.GuessLetters.Stars + data.MatchLetters.Stars +
            GetRightWrongMissingDataStars(data.PopTheLetters));

        float average = (sum / 12) * 100;

        return (int)average;
    }

    public int GetRightWrongMissingDataStars(RightWrongMissingData data)
    {
        int stars = 0;

        if ((data.Right > data.Wrong) && (data.Right > data.Missing) && ((data.Wrong == 0) || (data.Missing == 0)))
            stars = 3;
        else if ((data.Right > data.Wrong) && (data.Right > data.Missing) && (data.Wrong != 0) && (data.Missing != 0))
            stars = 2;
        else if ((data.Right < data.Wrong) || (data.Right < data.Missing))
            stars = 1;

        return stars;
    }

    public int Average(MathsData data)
    {
        float sum = 0;

        sum += (data.Counting.Stars + data.Sorting.Stars +
            data.AdditionSubtraction.Stars + GetRightWrongStarDataStars(data.Matching));

        float average = (sum / 12) * 100;
        return (int)average;
    }

    public int GetRightWrongStarDataStars(RightWrongStarData data)
    {
        int stars = 0;

        if (data.Right > data.Wrong && data.Stars == 3)
            stars = 3;
        else if (data.Right > data.Wrong && data.Stars == 2)
            stars = 2;
        else if (data.Right > data.Wrong && data.Stars == 1)
            stars = 1;

        return stars;
    }

    public int Average(UrduData data)
    {
        float sum = 0;
        foreach (TypeNameData nameStars in data.HaroofLikhiye)
        {
            sum += (float)nameStars.Stars;
        }
        sum = (int)(sum / 30) * 3;

        sum += (data.HaroofETahaji.Stars + data.HaroofMilaye.Stars);

        float average = (sum / 9) * 100;
        return (int)average;
    }

    public float FindPercentage(List<float> values, int index)
    {
        float totalCount = 0;

        for (int i = 0; i < values.Count; i++)
        {
            totalCount += values[i];
        }
        return values[index] / totalCount;
    }
}
    