using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkinToolEnums;

[CreateAssetMenu(fileName = "Armor", menuName = "Armor")]
public class Armor : ScriptableObject
{
    public Slot slot;

    public ArmorMaterial material;

    public Texture2D texture;

    public Texture2D icon;
}
