using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CommonUtil
{
    /// <summary>
    /// Converts bool value true/false to int value 0/1.
    /// </summary>
    /// <returns>The int value.</returns>
    /// <param name="value">The bool value.</param>
    public static int TrueFalseBoolToZeroOne(bool value)
    {
        if (value)
        {
            return 1;
        }
        return 0;
    }

    /// <summary>
    /// Converts int value 0/1 to bool value true/false.
    /// </summary>
    /// <returns>The bool value.</returns>
    /// <param name="value">The int value.</param>
    public static bool ZeroOneToTrueFalseBool(int value)
    {
        if (value == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Set the size for the UI element.
    /// </summary>
    /// <param name="trans">The Rect transform referenced.</param>
    /// <param name="newSize">The New size.</param>
    public static void SetSize(RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    /// <summary>
    /// Covert RectTransform to screen space.
    /// </summary>
    /// <returns>The transform to screen space.</returns>
    /// <param name="transform">Transform.</param>
    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
    }

    /// <summary>
    /// Find the game objects of tag.
    /// </summary>
    /// <returns>The game objects of tag(Sorted by name).</returns>
    /// <param name="tag">Tag.</param>
    public static GameObject[] FindGameObjectsOfTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        Array.Sort(gameObjects, CompareGameObjects);
        return gameObjects;
    }

    /// <summary>
    /// Finds the direct child by tag.
    /// </summary>
    /// <returns>The child by tag.</returns>
    /// <param name="p">parent.</param>
    /// <param name="childTag">Child tag.</param>
    public static Transform FindChildByTag(Transform theParent, string childTag)
    {
        if (string.IsNullOrEmpty(childTag) || theParent == null)
        {
            return null;
        }

        foreach (Transform child in theParent)
        {
            if (child.tag == childTag)
            {
                return child;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds the direct children by tag.
    /// </summary>
    /// <returns>The children by tag.</returns>
    /// <param name="p">parent.</param>
    /// <param name="childrenTag">Child tag.</param>
    public static List<Transform> FindChildrenByTag(Transform theParent, string childTag)
    {
        List<Transform> children = new List<Transform>();

        if (string.IsNullOrEmpty(childTag) || theParent == null)
        {
            return children;
        }

        foreach (Transform child in theParent)
        {
            if (child.tag == childTag)
            {
                children.Add(child);
            }
        }

        return children;
    }

    /// <summary>
    /// Compares the game objects.
    /// </summary>
    /// <returns>The game objects.</returns>
    /// <param name="gameObject1">Game object1.</param>
    /// <param name="gameObject2">Game object2.</param>
    private static int CompareGameObjects(GameObject gameObject1, GameObject gameObject2)
    {
        return gameObject1.name.CompareTo(gameObject2.name);
    }

    /// <summary>
    /// Enable the childern of the given gameobject.
    /// </summary>
    /// <param name="gameObject">The Gameobject reference.</param>
    public static void EnableChildern(Transform gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        foreach (Transform child in gameObject)
        {
            child.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Converts RGBA string to RGBA Color , seperator is ',' 
    /// </summary>
    /// <returns>The RGBA Color.</returns>
    /// <param name="rgba">rgba string.</param>
    public static Color StringRGBAToColor(string rgba)
    {
        Color color = Color.clear;

        if (!string.IsNullOrEmpty(rgba))
        {
            try
            {
                string[] rgbaValues = rgba.Split(',');
                float red = float.Parse(rgbaValues[0]);
                float green = float.Parse(rgbaValues[1]);
                float blue = float.Parse(rgbaValues[2]);
                float alpha = float.Parse(rgbaValues[3]);

                color.r = red;
                color.g = green;
                color.b = blue;
                color.a = alpha;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        return color;
    }

    /// <summary>
    /// Convert Integer value to custom string format.
    /// </summary>
    /// <returns>The to string.</returns>
    /// <param name="value">Value.</param>
    public static string IntToString(int value)
    {
        if (value < 10)
        {
            return "0" + value;
        }
        return value.ToString();
    }

    /// <summary>
    /// Disable the childern of the given gameobject.
    /// </summary>
    /// <param name="gameObject">The Gameobject reference.</param>
    public static void DisableChildern(Transform gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        foreach (Transform child in gameObject)
        {
            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Play the one shot clip.
    /// </summary>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="postion">Postion.</param>
    /// <param name="volume">Volume.</param>
    public static void PlayOneShotClipAt(AudioClip audioClip, Vector3 postion, float volume)
    {

        if (audioClip == null || volume == 0)
        {
            return;
        }

        GameObject oneShotAudio = new GameObject("one shot audio");
        oneShotAudio.transform.position = postion;

        AudioSource tempAudioSource = oneShotAudio.AddComponent<AudioSource>(); //add an audio source
        tempAudioSource.clip = audioClip;//set the audio clip
        tempAudioSource.volume = volume;//set the volume
        tempAudioSource.loop = false;//set loop to false
        tempAudioSource.rolloffMode = AudioRolloffMode.Linear;//linear rolloff mode
        tempAudioSource.Play();// play audio clip
        GameObject.Destroy(oneShotAudio, audioClip.length); //destroy oneShotAudio gameobject after clip duration
    }

    /// <summary>
    /// Get random color.
    /// </summary>
    /// <returns>The random color.</returns>
    public static Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), UnityEngine.Random.Range(0, 255), 255) / 255.0f;
    }
}



[DisallowMultipleComponent]
public class WinDialog : MonoBehaviour
{
    /// <summary>
    /// Number of stars for the WinDialog.
    /// </summary>
    private StarsNumber starsNumber;

    /// <summary>
    /// Number of stars Appears for the WinDialog.
    /// </summary>
    public int Stars = -1;

    /// <summary>
    /// Star sound effect.
    /// </summary>
    public AudioClip starSoundEffect;

    /// <summary>
    /// Win dialog animator.
    /// </summary>
    public Animator WinDialogAnimator;

    /// <summary>
    /// First star fading animator.
    /// </summary>
    public Animator firstStarFading;

    /// <summary>
    /// Second star fading animator.
    /// </summary>
    public Animator secondStarFading;

    /// <summary>
    /// Third star fading animator.
    /// </summary>
    public Animator thirdStarFading;

    /// <summary>
    /// The level title text.
    /// </summary>
    public TextMeshProUGUI levelTitle;

    /// <summary>
    /// The timer reference.
    /// </summary>
    public Timer timer;

    /// <summary>
    /// The effects audio source.
    /// </summary>
    private AudioSource effectsAudioSource;

    // Use this for initialization
    void Start()
    {
        Stars = 0;

        ///Setting up the references
        if (WinDialogAnimator == null)
        {
            WinDialogAnimator = GetComponent<Animator>();
        }

        if (firstStarFading == null)
        {
            firstStarFading = transform.Find("Stars").Find("FirstStarFading").GetComponent<Animator>();
        }

        if (secondStarFading == null)
        {
            secondStarFading = transform.Find("Stars").Find("SecondStarFading").GetComponent<Animator>();
        }

        if (thirdStarFading == null)
        {
            thirdStarFading = transform.Find("Stars").Find("ThirdStarFading").GetComponent<Animator>();
        }

        if (effectsAudioSource == null)
        {
            //effectsAudioSource = GameObject.Find("AudioSources").GetComponents<AudioSource>()[1];
        }

        if (levelTitle == null)
        {
            //levelTitle = transform.Find("Level").GetComponent<TextMeshProUGUI>();
        }

        if (timer == null)
        {
            timer = GameObject.Find("Time").GetComponent<Timer>();
        }
    }


    public void ShowResults()
    {
        Show();
        timer.Stop();
    }

    public void ResetResult()
    {
        Hide();
        timer.Reset();
    }

    /// <summary>
    /// When the GameObject becomes visible
    /// </summary>
    void OnEnable()
    {
        //Hide the Win Dialog
        Hide();
    }

    /// <summary>
    /// Show the Win Dialog.
    /// </summary>
    public void Show()
    {
        if (WinDialogAnimator == null)
        {
            return;
        }

        StarsNumber currentStarNumber = timer.progress.starsNumber;
        if (currentStarNumber == StarsNumber.ONE)
            Stars = 1;
        else if (currentStarNumber == StarsNumber.TWO)
            Stars = 2;
        else if (currentStarNumber == StarsNumber.THREE)
            Stars = 3;

        WinDialogAnimator.SetTrigger("Running");
    }

    /// <summary>
    /// Hide the Win Dialog.
    /// </summary>
    public void Hide()
    {
        StopAllCoroutines();
        Stars = 0;
        WinDialogAnimator.SetBool("Running", false);
        firstStarFading.SetBool("Running", false);
        secondStarFading.SetBool("Running", false);
        thirdStarFading.SetBool("Running", false);
    }

    /// <summary>
    /// Fade stars Coroutine.
    /// </summary>
    /// <returns>The stars.</returns>
    public IEnumerator FadeStars()
    {
        starsNumber = timer.progress.starsNumber;

        float delayBetweenStars = 0.5f;
        if (starsNumber == StarsNumber.ONE)
        {
            //Fade with One Star
            if (effectsAudioSource != null)
                CommonUtil.PlayOneShotClipAt(starSoundEffect, Vector3.zero, effectsAudioSource.volume);
            firstStarFading.SetTrigger("Running");
            ShowEffect(firstStarFading.transform);
        }
        else if (starsNumber == StarsNumber.TWO)
        {
            //Fade with Two Stars
            if (effectsAudioSource != null)
                CommonUtil.PlayOneShotClipAt(starSoundEffect, Vector3.zero, effectsAudioSource.volume);
            firstStarFading.SetTrigger("Running");
            ShowEffect(firstStarFading.transform);
            yield return new WaitForSeconds(delayBetweenStars);
            if (effectsAudioSource != null)
                CommonUtil.PlayOneShotClipAt(starSoundEffect, Vector3.zero, effectsAudioSource.volume);
            secondStarFading.SetTrigger("Running");
            ShowEffect(secondStarFading.transform);
        }
        else if (starsNumber == StarsNumber.THREE)
        {
            //Fade with Three Stars
            if (effectsAudioSource != null)
                CommonUtil.PlayOneShotClipAt(starSoundEffect, Vector3.zero, effectsAudioSource.volume);
            firstStarFading.SetTrigger("Running");
            ShowEffect(firstStarFading.transform);
            yield return new WaitForSeconds(delayBetweenStars);
            if (effectsAudioSource != null)
                CommonUtil.PlayOneShotClipAt(starSoundEffect, Vector3.zero, effectsAudioSource.volume);
            secondStarFading.SetTrigger("Running");
            ShowEffect(secondStarFading.transform);
            yield return new WaitForSeconds(delayBetweenStars);
            if (effectsAudioSource != null)
                CommonUtil.PlayOneShotClipAt(starSoundEffect, Vector3.zero, effectsAudioSource.volume);
            thirdStarFading.SetTrigger("Running");
            ShowEffect(thirdStarFading.transform);

        }
        yield return 0;
    }

    /// <summary>
    /// Show sub stars effect.
    /// </summary>
    /// <param name="fadingStar">Fading star.</param>
    private void ShowEffect(Transform fadingStar)
    {
        if (fadingStar == null)
        {
            return;
        }
        StartCoroutine(ShowEffectCouroutine(fadingStar));
    }

    /// <summary>
    /// Shows sub stars effect couroutine.
    /// </summary>
    /// <returns>The effect couroutine.</returns>
    /// <param name="fadingStar">Fading star reference.</param>
    private IEnumerator ShowEffectCouroutine(Transform fadingStar)
    {
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// Set the level title.
    /// </summary>
    /// <param name="value">Value.</param>
    public void SetLevelTitle(string value)
    {
        if (string.IsNullOrEmpty(value) || levelTitle == null)
        {
            return;
        }
        levelTitle.text = value;
    }

    public enum StarsNumber
    {
        ONE,
        TWO,
        THREE
    }
}
