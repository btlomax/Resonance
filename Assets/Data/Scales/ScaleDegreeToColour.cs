using UnityEngine;

/// <summary>
/// Provides a mapping from musical scale degrees to corresponding colors.
/// </summary>
/// <remarks>This ScriptableObject enables the association of specific colors with each scale degree, typically
/// for use in music visualization or educational tools. The mapping can be configured in the Unity Editor and reused
/// across multiple scenes or components.</remarks>
[CreateAssetMenu(fileName = "ScaleDegreeToColour", menuName = "Scriptable Objects/ScaleDegreeToColour")]
public class ScaleDegreeToColour : ScriptableObject
{
    public Color[] scaleDegreeColours = new Color[7];

    public Color Get(int scaleDegree)
    {
        return scaleDegreeColours[scaleDegree];
    }
}
