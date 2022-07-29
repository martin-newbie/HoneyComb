using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character", menuName = "Character", order = int.MinValue)]
public class CharacterScript : ScriptableObject
{
    public EPlayableCharacter characterType;
    public Sprite Icon;
    public string characterName;
    [TextArea]
    public string Description;
}

