using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Engine
{
    public enum GameStatus
    {
        SuccessfulMove,
        ChangedDirection,
        SteppedOnAMine,
        HitTheWall,
        ReachedExit,
        OutOfMoves,
        SkippingStep
    }
}
