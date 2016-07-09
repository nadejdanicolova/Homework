﻿using System;
using System.Collections.Generic;

namespace AcademyRPG
{
    public class Ninja : Character, IVulnerable, IFighter, IGatherer
    {
        private const int AttackPointsDefault = 0;
        private const int DefensePointsDefault = 0;
        private const int HitPointsDefault = 1;

        private int attackPoints;
        private int defensePoints;
        private int hitPoints;

        public Ninja(string name, Point position, int owner)
            : base(name, position, owner)
        {
            this.hitPoints = Ninja.HitPointsDefault;
            this.attackPoints = Ninja.AttackPointsDefault;
            this.defensePoints = Ninja.DefensePointsDefault;

            base.HitPoints = int.MaxValue;
        }

        public int AttackPoints
        {
            get
            {
                return this.attackPoints;
            }
        }

        public int DefensePoints
        {
            get
            {
                return this.defensePoints;
            }
        }

        public new int HitPoints
        {
            get
            {
                return this.hitPoints;
            }

            set
            {
                // do nothing, unit cannot be killed
                base.HitPoints = int.MaxValue;
            }
        }

        public new bool IsDestroyed
        {
            get { return false; }
        }

        /// <summary>
        /// The Ninja should always attack the target,
        /// which is not neutral, does not belong to the same player,
        /// and has the highest HitPoints of all the available targets.
        /// </summary>
        /// <param name="availableTargets"></param>
        /// <returns></returns>
        public int GetTargetIndex(List<WorldObject> availableTargets)
        {
            var potentialTargets = new List<WorldObject>();

            var maxHitPoints = int.MinValue;
            var maxHitPointsIndex = int.MinValue;

            for (int i = 0; i < availableTargets.Count; i++)
            {
                if (availableTargets[i].Owner != this.Owner
                    && availableTargets[i].Owner != 0
                    && (availableTargets[i] as Character).Name != this.Name)
                {
                    potentialTargets.Add(availableTargets[i]);

                    if (availableTargets[i].HitPoints > maxHitPoints)
                    {
                        maxHitPoints = availableTargets[i].HitPoints;
                        maxHitPointsIndex = i;
                    }
                }
            }

            if (potentialTargets.Count == 0)
            {
                return -1;
            }

            return maxHitPointsIndex;
        }

        /// <summary>
        /// The Ninja should be able to gather stone and lumber resources.
        /// For each lumber resource the Ninja has gathered, 
        /// its AttackPoints should increase by the resource’s quantity. 
        /// For each stone resource the Ninja has gathered, 
        /// its AttackPoints should increase by the resource’s
        /// quantity multiplied by 2.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool TryGather(IResource resource)
        {
            if (resource.Type == ResourceType.Lumber)
            {
                this.attackPoints += resource.Quantity;
                return true;
            }

            if (resource.Type == ResourceType.Stone)
            {
                this.attackPoints += resource.Quantity * 2;
                return true;
            }

            return false;
        }
    }
}
