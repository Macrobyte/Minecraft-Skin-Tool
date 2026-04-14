using System.Collections.Generic;
using UnityEngine;

public class PaintCommand : ICommand
{
    private Texture2D _texture;
    private List<PixelData> _pixelData;
    private Color _newColor;


    public PaintCommand(Texture2D texture, List<PixelData> pixelData)
    {
        _texture = texture;
        _pixelData = pixelData;
    }
    
    public void Execute()
    {
        foreach (var pixel in _pixelData)
        {
            _texture.SetPixel(pixel.Position.x, pixel.Position.y, pixel.AppliedColor);
        }
        
        _texture.Apply();
    }

    public void Undo()
    {
        foreach (var pixel in _pixelData)
        {
            _texture.SetPixel(pixel.Position.x, pixel.Position.y, pixel.OriginalColor);
        }
        
        _texture.Apply();
    }
}
