using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    [SerializeField] float ThreeStarsCount = 20;
    [SerializeField] float TwoStarsCount = 30;

    /// <summary>
    /// The star off sprite.
    /// </summary>
    public Sprite starOff;

    /// <summary>
    /// The star on sprite.
    /// </summary>
    public Sprite starOn;

    /// <summary>
    /// The level stars.
    /// </summary>
    public Image[] levelStars;

    /// <summary>
    /// The progress image.
    /// </summary>
    public Image progressImage;

    /// <summary>
    /// The stars number.
    /// </summary>
    [HideInInspector]
    public WinDialog.StarsNumber starsNumber;

    // Use this for initialization
    void Start()
    {
        if (progressImage == null)
        {
            progressImage = GetComponent<Image>();
        }
    }


    /// <summary>
    /// Set the value of the progress.
    /// </summary>
    /// <param name="currentTime">Current time.</param>
    public void SetProgress(float currentTime)
    {
        if (progressImage != null)
            progressImage.fillAmount = 1 - (currentTime / (TwoStarsCount * 1.0f + 1));

        if (currentTime >= 0 && currentTime <= ThreeStarsCount) // 3 Stars
        {
            if (levelStars[0] != null)
            {
                levelStars[0].sprite = starOn;
            }
            if (levelStars[1] != null)
            {
                levelStars[1].sprite = starOn;
            }
            if (levelStars[2] != null)
            {
                levelStars[2].sprite = starOn;
            }
            if (progressImage != null)
                progressImage.color = Color.green;

            starsNumber = WinDialog.StarsNumber.THREE;
        }
        else if (currentTime > ThreeStarsCount && currentTime <= TwoStarsCount) // 2 Stars
        {
            if (levelStars[2] != null)
            {
                levelStars[2].sprite = starOff;
            }
            if (progressImage != null)
                progressImage.color = Color.yellow;
            starsNumber = WinDialog.StarsNumber.TWO;

        }
        else   // 1 Star
        {
            if (levelStars[1] != null)
            {
                levelStars[1].sprite = starOff;
            }
            if (levelStars[2] != null)
            {
                levelStars[2].sprite = starOff;
            }
            if (progressImage != null)
                progressImage.color = Color.red;
            starsNumber = WinDialog.StarsNumber.ONE;
        }
    }

}
