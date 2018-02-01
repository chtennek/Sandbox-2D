using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Actor")]
public class Actor : ScriptableObject
{
    public Sprite defaultSprite;
    public string actorName;

    public Sprite[] sprites;
}
