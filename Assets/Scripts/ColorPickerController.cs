using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ColorPickerController : MonoBehaviour
{
    public float currentHue;
    public float currentSaturation;
    public float currentyValue;

    public RawImage hueImage;
    public RawImage SVImage;
    public RawImage outputImage;

    public Slider hueSlider;

    public TMP_InputField hexInputField;

    private Texture2D hueTexture;
    private Texture2D SVTexture;
    private Texture2D outputTexture;

    public Color colorPicked;

    public Action<Color> onColorChanged;


    private void Start()
    {
        CreateHueImage();

        CreateSVImage();

        CreateOutputImage();

        UpdateOutputImage();
    }

    private void CreateHueImage()
    {
        // Create a new Texture2D for the hue image
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int i = 0; i < hueTexture.height; i++)
        {
            // Calculate the hue value based on the pixel's position in the texture
            float hue = (float)i / hueTexture.height;

            // Convert the hue value to a Color using HSV to RGB conversion
            Color color = Color.HSVToRGB(hue, 1f, 1f);

            // Set the pixel color in the texture at the corresponding position
            hueTexture.SetPixel(0, i, color);
        }

        hueTexture.Apply();

        currentHue = 0f;

        // Assign the generated hue texture to the RawImage component to display it in the UI
        hueImage.texture = hueTexture;
    }

    private void CreateSVImage()
    {    
        SVTexture = new Texture2D(16, 16);
        SVTexture.wrapMode = TextureWrapMode.Clamp;
        SVTexture.name = "SaturationTexture";

        // Iterate through each pixel in the saturation texture and set its color based on the current hue, saturation, and value
        // It calculates the saturation based on x-axis and the value based on the y-axis
        for (int y = 0; y < SVTexture.height; y++)
        {
            for (int x = 0; x < SVTexture.width; x++)
            {
                float saturation = (float)x / SVTexture.width;

                float value = (float)y / SVTexture.height;

                Color color = Color.HSVToRGB(currentHue, saturation, value);

                SVTexture.SetPixel(x, y, color);
            }
        }

        SVTexture.Apply();

        currentSaturation = 0f;

        currentyValue = 0f;

        SVImage.texture = SVTexture;
    }

    private void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentyValue);

        for(int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        outputImage.texture = outputTexture;
    }

    private void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentyValue);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        hexInputField.text = ColorUtility.ToHtmlStringRGB(currentColor);

        colorPicked = currentColor;

        onColorChanged?.Invoke(colorPicked);
    }

    public void SetSV(float s, float v)
    {
        currentSaturation = s;

        currentyValue = v;

        UpdateOutputImage();
    }

    public void UpdateSVImage()
    {
        currentHue = hueSlider.value;

        for(int y = 0; y < SVTexture.height; y++)
        {
            for (int x = 0; x < SVTexture.width; x++)
            {
                float saturation = (float)x / SVTexture.width;

                float value = (float)y / SVTexture.height;

                Color color = Color.HSVToRGB(currentHue, saturation, value);

                SVTexture.SetPixel(x, y, color);
            }
        }

        SVTexture.Apply();

        UpdateOutputImage();
    }


    public void OnTextInput()
    {
        if(hexInputField.text.Length < 6)
        {
            return;
        }

        Color newColor;

        if(ColorUtility.TryParseHtmlString("#" + hexInputField.text, out newColor))
        {
            Color.RGBToHSV(newColor, out currentHue, out currentSaturation, out currentyValue);

            hueSlider.value = currentHue;

            hexInputField.text = "";

            UpdateOutputImage();

        }
    }
}
