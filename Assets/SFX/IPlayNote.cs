using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SFX
{
    public interface IPlayNote
    {
        void PlayNote(float note, float duration);
    }
}
