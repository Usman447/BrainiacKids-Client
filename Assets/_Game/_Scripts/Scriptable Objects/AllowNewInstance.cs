using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Allow Instance", menuName = "Scriptable Objects/Allow Instace", order = 1)]
public class AllowNewInstance : ScriptableObject
{
    public bool isAllowedNewInstance;
}
