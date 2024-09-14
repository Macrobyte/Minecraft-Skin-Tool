using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Armor")]
public class Armor : ScriptableObject
{

 #region Enums
    public enum Material
    {
        Chainmail,
        Iron,
        Diamond,
        Gold,
        Netherite,
        Turtle,
        Leather,
        None
    }
    #endregion

    // TODO: Slot probably shooldn't be a part of the PlayerModelHandler class
    public PlayerModelHandler.Slot slot;

    public Material material;

    public Texture2D texture;

    public Texture2D icon;
}
