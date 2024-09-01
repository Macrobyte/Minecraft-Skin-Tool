using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button saveCurrentButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;


    private void Awake()
    {
        saveCurrentButton.onClick.AddListener(SaveCurrent);
        quitButton.onClick.AddListener(Quit);
        backButton.onClick.AddListener(Back);
    }

    private void Back()
    {
        ToggleWindow();
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void SaveCurrent()
    {
        throw new NotImplementedException();
    }

    public void ToggleWindow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
