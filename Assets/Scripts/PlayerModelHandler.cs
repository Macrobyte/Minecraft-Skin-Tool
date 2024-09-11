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

public enum Slot
{
    Head,
    Chest,
    Legs,
    Feet
}

public class PlayerModelHandler : MonoBehaviour
{
    public static PlayerModelHandler Instance { get; private set; }

    [Space(5)]
    [Category("Shader and Materials", TextAnchor.MiddleCenter)]
    [Space(10)]

    [SerializeField] private Shader texturesShader;
    [Header("Skin")]
    [SerializeField] private Material innerSkinMaterial;
    [SerializeField] private Material outerSkinMaterial;
    [SerializeField, ReadOnly] private List<Material> allSkinMaterials = new List<Material>();

    [Header("Armor")]
    [SerializeField] private Material helmetMaterial;
    [SerializeField] private Material chestplateMaterial;
    [SerializeField] private Material leggingsMaterial;
    [SerializeField] private Material bootsMaterial;
    [SerializeField, ReadOnly] private List<Material> allArmorMaterials = new List<Material>();

    [Space(10)]
    [Divider(4, 88, 88, 88, 0.5f)]
    [Category("Model Info", TextAnchor.MiddleCenter)]

    [Header("Armor")]
    [SerializeField, ReadOnly] private GameObject[] allArmor;

    [Header("Arms")]
    [SerializeField, ReadOnly] private GameObject[] steveArms;
    [SerializeField, ReadOnly] private GameObject[] alexArms;


    [Space(10)]
    [Divider(4, 88, 88, 88, 0.5f)]
    [Category("Skin", TextAnchor.MiddleCenter)]
    [Space(10)]

    [SerializeField] private Texture2D defaultSkin;
    [SerializeField] private Texture2D currentSkin;

    [Space(10)]
    [Category("Armor", TextAnchor.MiddleCenter)]
    [Space(10)]

    [Header("Armor Inventory")]
    [SerializeField] private List<Armor> Helmets;
    [SerializeField] private List<Armor> Chestplates;
    [SerializeField] private List<Armor> Leggings;
    [SerializeField] private List<Armor> Boots;

    [Header("Equiped Armor")]
    [SerializeField, ReadOnly] private Armor equippedHelmet;
    [SerializeField, ReadOnly] private Armor equippedChestplate;
    [SerializeField, ReadOnly] private Armor equippedLeggings;
    [SerializeField, ReadOnly] private Armor equippedBoots;

    

    public Action<Armor> onArmorEquipped;

    [Space(10)]
    [Divider(4, 88, 88, 88, 0.5f)]
    [Category("Model States", TextAnchor.MiddleCenter)]
    
    [SerializeField] private Model currentModel = Model.Steve;
 
    [SerializeField, ReadOnly] private Animator playerModelAnimator;

    public enum AnimationState
    {
        Idle,
        Walking,
        Detached
    }

    [SerializeField] private AnimationState currentAnimationState = AnimationState.Idle; 

    private void Awake()
    {
        playerModelAnimator = GetComponent<Animator>();

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
        FindAllSkinMaterials();

        FindAllArmObjects();

        FindAllArmorMaterials();

        FindAllArmorObjects();

        ApplySkin(defaultSkin);

        PlayAnimation(currentAnimationState);

        //DisableArmor();

    }

#region Skin Handling

    private void FindAllArmObjects()
    {
        steveArms = GameObject.FindGameObjectsWithTag("Steve Arm");
        alexArms = GameObject.FindGameObjectsWithTag("Alex Arm");
    }

    private void FindAllSkinMaterials()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshes)
        {
            if (mesh.material.name.Contains("Inner"))
            {
                mesh.material = innerSkinMaterial;

                allSkinMaterials.Add(mesh.material);
            }

            if (mesh.material.name.Contains("Outer"))
            {
                mesh.material = outerSkinMaterial;

                allSkinMaterials.Add(mesh.material);
            }
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
#endregion

#region Armor Handling
    private void FindAllArmorObjects()
    {
        allArmor = GameObject.FindGameObjectsWithTag("Armor");
    }

    private void FindAllArmorMaterials()
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshes)
        {
            if (mesh.material.name.Contains("Helmet"))
            {
                mesh.material = helmetMaterial;
                allArmorMaterials.Add(mesh.material);
            }

            if (mesh.material.name.Contains("Chestplate"))
            {
                mesh.material = chestplateMaterial;
                allArmorMaterials.Add(mesh.material);
            }

            if (mesh.material.name.Contains("Leggings"))
            {
                mesh.material = leggingsMaterial;
                allArmorMaterials.Add(mesh.material);
            }

            if (mesh.material.name.Contains("Boot"))
            {
                mesh.material = bootsMaterial;
                allArmorMaterials.Add(mesh.material);
            }
        }
    }

    private void Equip(Armor armor, Slot slot)
    {
        switch(slot)
            {
            case Slot.Head:
                foreach (Material material in allArmorMaterials)
                {
                    if(material.name.Contains("Helmet"))
                    {
                        material.mainTexture = armor.texture;
                        equippedHelmet = armor;

                    }
                }
                break;
            case Slot.Chest:
                foreach (Material material in allArmorMaterials)
                {
                    if (material.name.Contains("Chestplate"))
                    {
                        material.mainTexture = armor.texture;
                        equippedChestplate = armor;
                    }
                }             
                break;
            case Slot.Legs:
                foreach (Material material in allArmorMaterials)
                {
                    if (material.name.Contains("Leggings"))
                    {
                        material.mainTexture = armor.texture;
                        equippedLeggings = armor;
                    }
                }   
                break;
            case Slot.Feet:
                foreach (Material material in allArmorMaterials)
                {
                    if (material.name.Contains("Boot"))
                    {
                        material.mainTexture = armor.texture;
                        equippedBoots = armor;
                    }
                }   
                break;
        }
    }

    private void DisableArmor()
    {
        foreach (GameObject armor in allArmor)
        {
            armor.SetActive(false);
        }
    }

    private void EnableArmor()
    {
        foreach (GameObject armor in allArmor)
        {
            armor.SetActive(true);
        }
    }

    public void ToggleArmor(bool state)
    {
        if (state)
        {
            EnableArmor();
        }
        else
        {
            DisableArmor();
        }
    }

    public void ChangeArmor(string name, int v)
    {
        switch (name)
        {
            case "Helmet":
                int currentHelmetIndex = Helmets.IndexOf(equippedHelmet);
                int newHelmetIndex = (currentHelmetIndex + v) % Helmets.Count;

                // Handle wrapping around the list when going below 0
                if (newHelmetIndex < 0)
                {
                    newHelmetIndex += Helmets.Count;
                }

                // Equip the new helmet and update the equippedHelmet variable
                equippedHelmet = Helmets[newHelmetIndex];
                Equip(equippedHelmet, Slot.Head);
                onArmorEquipped?.Invoke(equippedHelmet);
                break;
            case "Chestplate":
                int currentChestplateIndex = Chestplates.IndexOf(equippedChestplate);
                int newChestplateIndex = (currentChestplateIndex + v) % Chestplates.Count;

                if (newChestplateIndex < 0)
                {
                    newChestplateIndex += Chestplates.Count;
                }

                equippedChestplate = Chestplates[newChestplateIndex];
                Equip(equippedChestplate, Slot.Chest);
                onArmorEquipped?.Invoke(equippedChestplate);
                break;
            case "Leggings":
                int currentLeggingsIndex = Leggings.IndexOf(equippedLeggings);
                int newLeggingsIndex = (currentLeggingsIndex + v) % Leggings.Count;

                if (newLeggingsIndex < 0)
                {
                    newLeggingsIndex += Leggings.Count;
                }

                equippedLeggings = Leggings[newLeggingsIndex];
                Equip(equippedLeggings, Slot.Legs);
                onArmorEquipped?.Invoke(equippedLeggings);
                break;
            case "Boots":
                int currentBootsIndex = Boots.IndexOf(equippedBoots);
                int newBootsIndex = (currentBootsIndex + v) % Boots.Count;

                if (newBootsIndex < 0)
                {
                    newBootsIndex += Boots.Count;
                }

                equippedBoots = Boots[newBootsIndex];
                Equip(equippedBoots, Slot.Feet);
                onArmorEquipped?.Invoke(equippedBoots);
                break;
        }
    }

    #endregion

    #region Model Control
    public void PlayAnimation(AnimationState state)
    {
        Debug.Log("Playing animation");

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
