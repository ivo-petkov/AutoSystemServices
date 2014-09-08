using System;

namespace AutoSystem.Models
{
    public enum RepairStatus
    {
        InProgress = 0,
        WaitingForParts = 1,
        ArriveParts = 2,
        Finished = 3,
    }
}
