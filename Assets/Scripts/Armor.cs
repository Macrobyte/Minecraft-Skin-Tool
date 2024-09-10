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
        Turtle
    }

    public enum Type
    {
        Helmet,
        Chestplate,
        Leggings,
        Boots
    }
#endregion

    public Material material;

    public Texture2D texture;

    public Texture2D icon;
}
