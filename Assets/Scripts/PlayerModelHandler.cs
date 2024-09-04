using System.Collections.Generic;
using UnityEngine;
using UnityReadOnly = Unity.Collections.ReadOnlyAttribute;
using CustomAttributes;
using System;

public enum Model
{
    Steve,
    Alex
}

//[Serializable]
//enum ArmorMaterial
//{
//    None,
//    Chainmail,
//    Iron,
//    Gold,
//    Diamond,
//    Netherite,
//    Turtle
//}

//[Serializable]
//struct Armor
//{
//    public Texture2D armorTexture;
//    public ArmorMaterial armorMaterial;
//}


public class PlayerModelHandler : MonoBehaviour
{
    public static PlayerModelHandler Instance { get; private set; }

    [Header("Shader and Materials")]
    [SerializeField] private Shader texturesShader;
    
    

    [Header("Skin")]
    [SerializeField] private Texture2D defaultSkin;

    [SerializeField] private Texture2D currentSkin;

    [SerializeField, ReadOnly] private List<Material> allSkinMaterials = new List<Material>();

    //[Header("Armor")]
    //[SerializeField] private Armor Helmet;
    //[SerializeField] private Armor Chestplate;
    //[SerializeField] private Armor Leggings;
    //[SerializeField] private Armor Boots;

    
    [Header("Model Controller")]
    [SerializeField, ReadOnly] private float defaultRotationSpeed = 100.0f;

    [SerializeField, ReadOnly] private float currentRotationSpeed;

    [SerializeField] private Model currentModel = Model.Steve;

    [Header("Model Animator")]
    [SerializeField, ReadOnly] private Animator playerModelAnimator;

    public enum AnimationState
    {
        Idle,
        Walking,
        Detached
    }

    [SerializeField] private AnimationState currentAnimationState = AnimationState.Idle; 
 
    private GameObject[] steveArms;

    private GameObject[] alexArms;

    private void Awake()
    {
        playerModelAnimator = GetComponent<Animator>();

        currentRotationSpeed = defaultRotationSpeed;

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        FindAllMaterialsInModel();

        FindAllArms();

        ApplySkin(defaultSkin);

        PlayAnimation(currentAnimationState);
    }

#region Model Material Handling
    /// <summary>
    /// This method is mostly used by the SkinSearchTool which gets Texture2D and Model information from the MojangAPIHandler.
    /// </summary>
    /// <param name="skin"></param>
    /// <param name="model"></param>
    public void ApplySkin(Texture2D skin, Model model)
    {
        ChangeModel(model);

        currentSkin = skin;

        // Apply the skin to the materials of the model
        foreach (Material material in allSkinMaterials)
        {
            material.mainTexture = currentSkin;
        }
    }

    /// <summary>
    /// General ApplySkin method that requires only the Texture2D.
    /// </summary>
    /// <param name="skin"></param>
    public void ApplySkin(Texture2D skin)
    {
        Debug.Log("Applying skin");

        ChangeModel(Model.Steve);

        currentSkin = skin;

        
        foreach (Material material in allSkinMaterials)
        {
            material.mainTexture = currentSkin;
        }
    }

    private void ResetSkin(Texture2D defaultSkin)
    {
        currentSkin = defaultSkin;

        // Apply the default skin to the materials of the model
        foreach (Material material in allSkinMaterials)
        {
            material.mainTexture = currentSkin;
        }
        
    }

    private void ChangeModel(Model newModel)
    {

        foreach (GameObject arm in steveArms)
        {
            arm.SetActive(newModel == Model.Steve);
        }

        foreach (GameObject arm in alexArms)
        {
            arm.SetActive(newModel == Model.Alex);
        }

        currentModel = newModel;
    }

    public void ToggleModel()
    {
        if (currentModel == Model.Steve)
        {
            ChangeModel(Model.Alex);
        }
        else
        {
            ChangeModel(Model.Steve);
        }
    }

    private void FindAllMaterialsInModel()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshes)
        {
            if(mesh.material.name.Contains("Inner") || mesh.material.name.Contains("Outer"))
            {
                Debug.Log("Found: " + mesh.material.name);
                // TODO: Add armor to alex model in blender so that the materials are present in slim arm models
            }


            allSkinMaterials.Add(mesh.material);
        }
    }

    private void FindAllArms()
    {

       steveArms = GameObject.FindGameObjectsWithTag("Steve Arm");
        alexArms = GameObject.FindGameObjectsWithTag("Alex Arm");
    }

#endregion

#region Model Control

    public void PlayAnimation(AnimationState state)
    {
        // switch statement the state of the animation
        switch (state)
        {
            case AnimationState.Idle:
                playerModelAnimator.SetBool("isIdle", true);
                playerModelAnimator.SetBool("isWalking", false);
                playerModelAnimator.SetBool("isDetached", false);
                break;
            case AnimationState.Walking:
                playerModelAnimator.SetBool("isIdle", false);
                playerModelAnimator.SetBool("isWalking", true);
                playerModelAnimator.SetBool("isDetached", false);
                break;
            case AnimationState.Detached:
                playerModelAnimator.SetBool("isIdle", false);
                playerModelAnimator.SetBool("isWalking", false);
                playerModelAnimator.SetBool("isDetached", true);
                break;
        }
    }
#endregion

#region Getters

    public Texture2D GetCurrentSkin()
    {
        return currentSkin;
    }

#endregion
}
