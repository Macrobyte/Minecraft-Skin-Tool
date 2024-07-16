using System.Collections.Generic;
using UnityEngine;
public enum Model
{
    Steve,
    Alex
}

public class PlayerModelHandler : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField]
    private Shader skinShader;
    
    public Texture2D defaultSkin;

    [SerializeField]
    private Texture2D currentSkin;

    [SerializeField]
    private List<Material> allMaterialsInModel = new List<Material>();

    [Header("Model Controller")]
    private float defaultRotationSpeed = 100.0f;

    [SerializeField]
    private float currentRotationSpeed;

    [SerializeField]
    private Animator playerModelAnimator;

    public enum RotationDirection
    {
        Right,
        Left
    }

    [SerializeField]
    private RotationDirection rotationDirection = RotationDirection.Right;

    [SerializeField]
    private Model currentModel = Model.Steve;

    [SerializeField]
    private GameObject[] steveArms;

    [SerializeField]
    private GameObject[] alexArms;

    private void Awake()
    {
        playerModelAnimator = GetComponent<Animator>();

        currentRotationSpeed = defaultRotationSpeed;
    }

    private void Start()
    {
        FindAllMaterialsInModel();

        FindAllArms();

        ToggleModel();
    }

    private void Update()
    {
        RotateModel(rotationDirection);
    }


    #region Model Material Handling
    public void ApplySkin(Texture2D skin, Model model)
    {
        switch (model)
        {
            case Model.Steve:
                currentModel = Model.Steve;
                ToggleModel();
                break;
            case Model.Alex:
                currentModel = Model.Alex;
                ToggleModel();
                break;
        }

        currentSkin = skin;

        // Apply the skin to the materials of the model
        foreach (Material material in allMaterialsInModel)
        {
            material.mainTexture = currentSkin;
        }
    }

    public void ResetSkin(Texture2D defaultSkin)
    {
        currentSkin = defaultSkin;

        // Apply the default skin to the materials of the model
        foreach (Material material in allMaterialsInModel)
        {
            material.mainTexture = currentSkin;
        }
        
    }

    public void ToggleModel()
    {
        // Set the current model to the opposite of the current model
        currentModel = currentModel == Model.Steve ? Model.Alex : Model.Steve;

        // Toggle the arms
        foreach (GameObject arm in steveArms)
        {
            arm.SetActive(currentModel == Model.Alex);
        }

        foreach (GameObject arm in alexArms)
        {
            arm.SetActive(currentModel == Model.Steve);
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
    }

    public void SetRotationDirection(RotationDirection direction)
    {
        rotationDirection = direction;
    }

    #endregion
}
