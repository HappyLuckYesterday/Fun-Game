﻿using Rhisis.Database.Structures;

namespace Rhisis.World.Game.Components
{
    public class StatisticsComponent
    {
        public ushort Strenght { get; set; }

        public ushort Stamina { get; set; }

        public ushort Dexterity { get; set; }

        public ushort Intelligence { get; set; }

        public ushort StatPoints { get; set; }

        public StatisticsComponent(Character character)
        {
            this.Strenght = (ushort)character.Strength;
            this.Stamina = (ushort)character.Stamina;
            this.Dexterity = (ushort)character.Dexterity;
            this.Intelligence = (ushort)character.Intelligence;
            this.StatPoints = (ushort)character.StatPoints;
        }

        public StatisticsComponent()
        {
        }
    }
}
