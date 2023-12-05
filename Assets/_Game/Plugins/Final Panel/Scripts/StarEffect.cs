using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class StarEffect : MonoBehaviour
{
    /// <summary>
    /// The position of Stars Effect in the World Space.
    /// </summary>
    private Vector3 tempPosition;

    /// <summary>
    /// The stars effect prefab.
    /// </summary>
    public GameObject starsEffectPrefab;

    /// <summary>
    /// The star effect Z position.
    /// </summary>
    [Range(-50, 50)]
    public float starEffectZPosition = -5;

    /// <summary>
    /// The stars effect parent.
    /// </summary>
    public Transform starsEffectParent;

    /// <summary>
    /// Create the stars effect.
    /// </summary>
    public void CreateStarsEffect()
    {
        if (starsEffectPrefab == null)
        {
            return;
        }
        tempPosition = transform.position;
        tempPosition.z = starEffectZPosition;
        GameObject starsEffect = Instantiate(starsEffectPrefab, starsEffectParent);
        if (starsEffectParent != null)
            starsEffect.transform.localPosition = tempPosition;
        starsEffect.transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
