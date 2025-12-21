using UnityEngine;

[CreateAssetMenu(fileName = "ScaleDegreeToColour", menuName = "Scriptable Objects/ScaleDegreeToColour")]
public class ScaleDegreeToColour : ScriptableObject
{
    public Color[] scaleDegreeColours = new Color[7];

    public Color Get(int scaleDegree)
    {
        return scaleDegreeColours[scaleDegree];
    }
}
