using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public Ray beam;

    public Color color;
    public List<BeamSegment> segments = new();

    public Beam(Ray beam, Color beamColor)
    {
        beam = beam;
        color = beamColor;
    }
}

public struct BeamSegment
{
    public Vector3 origin;
    public Vector3 end;

    public BeamSegment(Vector3 origin, Vector3 end)
    {
        this.origin = origin;
        this.end = end;
    }
}
