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

    [SerializeField] private Material material;

    [SerializeField] private Type type;

    [SerializeField] private Texture2D texture;


  
}
