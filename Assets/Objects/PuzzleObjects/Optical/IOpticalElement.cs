using Assets.Objects.PuzzleObjects.Optical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Objects.PuzzleObjects
{
    public interface IOpticalElement
    {
        bool Interact(Ray incomingRay,
        RaycastHit hit,
        BeamContext beam,
        out Ray outgoingRay);
    }
}
