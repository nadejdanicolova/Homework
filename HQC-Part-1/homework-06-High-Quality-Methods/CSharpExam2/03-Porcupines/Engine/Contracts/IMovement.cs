﻿using _03_Porcupines.Animals.Enums;
using _03_Porcupines.Engine.Enums;

namespace _03_Porcupines.Engine.Contracts
{
    public interface IMovement
    {
        DirectionType DirectionType { get; }

        MovementType MovementType { get; }

        IPosition Delta { get; }
    }
}
