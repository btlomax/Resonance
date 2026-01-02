using UnityEngine;

public class BeamRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _baseWidth;
    [SerializeField] private bool _animatePulse = true;
    [SerializeField] private float _pulseSpeed;
    [SerializeField] private float _pulseAmplitude;
    [SerializeField] private Material _beamMaterial;
    [SerializeField] private Beam _currentBeam;
    private float _beamLifetime;

    public Transform beamOrigin;

    private void Awake()
    {
        if(_lineRenderer == null)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// Renders the given beam for the specified duration
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

        _currentBeam = beam;
        _beamLifetime = duration;

        // Setup LineRenderer positions
        _lineRenderer.positionCount = beam.segments.Count + 1;

        _beamMaterial.color = beam.color;

        _lineRenderer.SetPosition(0, beam.segments[0].origin);

        for (int i = 0; i < beam.segments.Count; i++)
        {
            _lineRenderer.SetPosition(i + 1, beam.segments[i].end);
        }

        _lineRenderer.startWidth = _baseWidth;
        _lineRenderer.endWidth = _baseWidth;
    }

    private void Update()
    {
        if (_currentBeam == null || beamOrigin == null)
            return;

        if(_currentBeam.segments.Count > 0)
            _currentBeam.segments.Clear();

        Ray ray = new Ray(beamOrigin.position, beamOrigin.forward);
        _currentBeam.lightBeam = ray;

        BeamManager.Instance.ProcessBeam(_currentBeam);

        // Update line positions
        _lineRenderer.positionCount = _currentBeam.segments.Count + 1; 
        _lineRenderer.SetPosition(0, _currentBeam.segments[0].origin);

        for (int i = 0; i < _currentBeam.segments.Count; i++)
        {
            _lineRenderer.SetPosition(i + 1, _currentBeam.segments[i].end);
        }

        // Pulse animation
        if (_animatePulse)
        {
            float pulse = _baseWidth + Mathf.Sin(Time.time * _pulseSpeed) * _pulseAmplitude;
            _lineRenderer.startWidth = pulse;
            _lineRenderer.endWidth = pulse;
        }

        // Reduce lifetime
        if (_beamLifetime > 0f)
        {
            _beamLifetime -= Time.deltaTime;
            if (_beamLifetime <= 0f)
            {
                Clear();
            }
        }
    }

    public void Clear()
    {
        _lineRenderer.positionCount = 0;
        _currentBeam = null;
    }
}
