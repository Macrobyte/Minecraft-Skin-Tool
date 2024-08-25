using System.Collections.Generic;
using UnityEngine;
public enum Model
{
    Steve,
    Alex
}

public class PlayerModelHandler : MonoBehaviour
{
    public static PlayerModelHandler Instance { get; private set; }

    [Header("Materials")]
    [SerializeField]
    private Shader skinShader;
    
    public Texture2D defaultSkin;

    [SerializeField]
    private Texture2D currentSkin;

    private List<Material> allMaterialsInModel = new List<Material>();

    [Header("Model Controller")]
    private float defaultRotationSpeed = 100.0f;

    //[SerializeField]
    private float currentRotationSpeed;

    [SerializeField]
    private Animator playerModelAnimator;

    public enum RotationDirection
    {
        Right,
        Left
    }

    //[SerializeField]
    private RotationDirection rotationDirection = RotationDirection.Right;

    [SerializeField]
    private Model currentModel = Model.Steve;
 
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
        foreach (Material material in allMaterialsInModel)
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

        
        foreach (Material material in allMaterialsInModel)
        {
            material.mainTexture = currentSkin;
        }
    }

    private void ResetSkin(Texture2D defaultSkin)
    {
        currentSkin = defaultSkin;

        // Apply the default skin to the materials of the model
        foreach (Material material in allMaterialsInModel)
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
            allMaterialsInModel.Add(mesh.material);
        }
    }

    private void FindAllArms()
    {

       steveArms = GameObject.FindGameObjectsWithTag("Steve Arm");
        alexArms = GameObject.FindGameObjectsWithTag("Alex Arm");
    }

    #endregion

    // Currently unused but keeping in case it is needed in the future. ToggleDetachMode() is still used.
    #region Model Control

    public void ToggleRotationSpeed()
    {
        // If the current rotation speed is the default speed, set it to 0
        if (currentRotationSpeed == defaultRotationSpeed)
        {
            currentRotationSpeed = 0;
        }
        else
        {
            currentRotationSpeed = defaultRotationSpeed;
        }
    }
    
    private void RotateModel(RotationDirection direction)
    {
        switch(direction)
        {
            case RotationDirection.Right:
                gameObject.transform.Rotate(Vector3.forward, -currentRotationSpeed * Time.deltaTime);
                break;
            case RotationDirection.Left:
                gameObject.transform.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
                break;
        }
    }

    public void ToggleDetachMode()
    {
        // Set a temporary variable to the opposite of the current state
        bool isDetached = !playerModelAnimator.GetBool("isDetached");

        // Set the animator bool to the new state
        playerModelAnimator.SetBool("isDetached", isDetached);

        Debug.Log("Detached mode: " + isDetached);
    }

    public void SetRotationDirection(RotationDirection direction)
    {
        rotationDirection = direction;
    }

    #endregion



    public void FARTING()
    {
        PlayerAudioManager.instance.PlayPlayerSound(PlayerSounds.Fart);
    }
}
