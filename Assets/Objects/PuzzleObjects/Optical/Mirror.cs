using Assets.Objects.PuzzleObjects;
using Assets.Objects.PuzzleObjects.Optical;
using UnityEngine;

public class Mirror : MonoBehaviour, IOpticalElement
{
    public bool Interact(Ray incomingRay, RaycastHit hit, BeamContext beam, out Ray outgoingRay)
    {
        throw new System.NotImplementedException();
    }
}
