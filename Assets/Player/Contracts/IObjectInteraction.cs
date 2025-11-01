using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Player.Contracts
{
    public interface IObjectInteraction
    {
        void Interact(GameObject interactor);
    }
}
