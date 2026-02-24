using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Data.GameManagement
{
    public enum GameUI_Event
    {
        None,
        ReachedApples,
        ApplesCollected,
        EnteredResonatorPuzzleArea,
        CompletedResonatorPuzzle,
        EnteredCopyPuzzleArea,
        CompletedCopyPuzzle,
        OpenedDoor,
        LevelCompleted
    }

    public enum GameUI_Errors
    {
        None,
        IncorrectHarmony,
        IncorrectSequence,
    }
}
