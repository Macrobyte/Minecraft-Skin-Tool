using UnityEngine;

public class PixelData
{
    public Vector2Int Position;
    public Color OriginalColor;
    public Color AppliedColor;

    public PixelData(Vector2Int position, Color originalColor, Color appliedColor)
    {
        Position = position;
        OriginalColor = originalColor;
        AppliedColor = appliedColor;
    }

}
