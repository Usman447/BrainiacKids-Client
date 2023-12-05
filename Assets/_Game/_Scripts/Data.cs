using JetBrains.Annotations;
using System;
using System.Collections.Generic;

#region Data Classes

[Serializable]
public class Data
{
    public EnglishData English;
    public MathsData Maths;
    public UrduData Urdu;

    public Data()
    {
        English = new EnglishData();
        Maths = new MathsData();
        Urdu = new UrduData();
    }
}

[Serializable]
public class EnglishData
{
    public List<TypeNameData> Tracking;
    public StarData MatchLetters;
    public StarData GuessLetters;
    public RightWrongMissingData PopTheLetters;

    public EnglishData()
    {
        Tracking = new List<TypeNameData>();
        MatchLetters = new StarData();
        GuessLetters = new StarData();
        PopTheLetters = new RightWrongMissingData();
    }
}

[Serializable]
public class UrduData
{
    public StarData HaroofETahaji;
    public List<TypeNameData> HaroofLikhiye;
    public StarData HaroofMilaye;

    public UrduData()
    {
        HaroofETahaji = new StarData();
        HaroofLikhiye = new List<TypeNameData>();
        HaroofMilaye = new StarData();
    }
}

[Serializable]
public class MathsData
{
    public StarData Counting;
    public StarData Sorting;
    public RightWrongStarData Matching;
    public StarData AdditionSubtraction;

    public MathsData()
    {
        Counting = new StarData();
        Sorting = new StarData();
        Matching = new RightWrongStarData();
        AdditionSubtraction = new StarData();
    }
}

[Serializable]
public class RightWrongStarData
{
    public int Stars;
    public int Right;
    public int Wrong;

    public RightWrongStarData() { }

    public RightWrongStarData(int _stars, int _right, int _wrong)
    {
        Stars = _stars;
        Right = _right;
        Wrong = _wrong;
    }
}

[Serializable]
public class StarData
{
    public int Stars;

    public StarData() { }

    public StarData(int _stars)
    {
        Stars = _stars;
    }
}

[Serializable]
public class RightWrongMissingData
{
    public int Right;
    public int Wrong;
    public int Missing;

    public RightWrongMissingData() { }

    public RightWrongMissingData(int right, int wrong, int missing)
    {
        Right = right;
        Wrong = wrong;
        Missing = missing;
    }
}

[Serializable]
public class TypeNameData
{
    public string Name;
    public int Stars;

    public TypeNameData() { }

    public TypeNameData(string name, int stars)
    {
        Name = name;
        Stars = stars;
    }
}

#endregion

[Serializable]
public class OverallProgress
{
    public List<StudentProgress> studentProgresses;

    public OverallProgress()
    {
        studentProgresses = new List<StudentProgress>();
    }
}

[Serializable]
public class StudentProgress
{
    public string studentName;
    public int progress;

    public StudentProgress(string studentName, int progress)
    {
        this.studentName = studentName;
        this.progress = progress;
    }
}
