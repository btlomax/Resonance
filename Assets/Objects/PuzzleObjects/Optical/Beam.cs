using System.Collections.Generic;
using UnityEngine;

public class Beam
{
    public Ray lightBeam;
    public float width;
    public float pulseFrequency;
    public Color color;
    public List<BeamSegment> segments = new List<BeamSegment>();

    public Beam(Ray beam, Color beamColor)
    {
        lightBeam = beam;
        color = beamColor;
    }

    public Beam(Ray beam, Color beamColor, float beamWidth, float beamPulseFrequency)
    {
         lightBeam = beam;
         color = beamColor;
         width = beamWidth;
         pulseFrequency = beamPulseFrequency;
    }
}

public readonly struct BeamSegment
{
    public readonly Vector3 origin;
    public readonly Vector3 end;

    public BeamSegment(Vector3 origin, Vector3 end)
    {
        this.origin = origin;
        this.end = end;
    }
}
