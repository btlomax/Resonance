using UnityEngine;

public class BeamRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float baseWidth;
    [SerializeField] private bool animatePulse = true;
    [SerializeField] private float pulseSpeed;
    [SerializeField] private float pulseAmplitude;

    [SerializeField] private Beam currentBeam;
    private float beamLifetime;

    private void Awake()
    {
        if(lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// Light not matching note colour
    /// </summary>
    /// <param name="beam"></param>
    /// <param name="duration"></param>
    public void RenderBeam(Beam beam, float duration)
    {
        if (beam == null || beam.segments.Count == 0)
        {
            Clear();
            return;
        }

        currentBeam = beam;
        beamLifetime = duration;

        // Setup LineRenderer positions
        lineRenderer.positionCount = beam.segments.Count + 1;
        lineRenderer.startColor = beam.color;
        lineRenderer.endColor = beam.color;

        lineRenderer.SetPosition(0, beam.segments[0].origin);

        for (int i = 0; i < beam.segments.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, beam.segments[i].end);
        }

        lineRenderer.startWidth = baseWidth;
        lineRenderer.endWidth = baseWidth;
    }

    private void Update()
    {
        if (currentBeam == null)
            return;

        // Pulse animation
        if (animatePulse)
        {
            float pulse = baseWidth + Mathf.Sin(Time.time * pulseSpeed) * pulseAmplitude;
            lineRenderer.startWidth = pulse;
            lineRenderer.endWidth = pulse;
        }

        // Reduce lifetime
        if (beamLifetime > 0f)
        {
            beamLifetime -= Time.deltaTime;
            if (beamLifetime <= 0f)
            {
                Clear();
            }
        }
    }

    public void Clear()
    {
        lineRenderer.positionCount = 0;
        currentBeam = null;
    }
}
