using System.Collections;
using System.Collections.Generic;
using HnSF.Combat;
using HnSF.Fighters;
using HnSF.Input;
using UnityEngine;

namespace HnSF.Sample.TDAction
{
    public class FighterCombatManager : MonoBehaviour, IFighterCombatManager
    {
        public int HitStop { get; protected set; } = 0;
        public int HitStun { get; protected set; } = 0;
        public int CurrentChargeLevel { get; protected set; } = 0;
        public int CurrentChargeLevelCharge { get; protected set; } = 0;

        public int team = 0;
        
        public void Cleanup()
        {
            CurrentChargeLevel = 0;
            CurrentChargeLevelCharge = 0;
        }
        
        public void SetHitStop(int value)
        {
            HitStop = value;
        }

        public void AddHitStop(int value)
        {
            HitStop += value;
        }

        public void SetHitStun(int value)
        {
            HitStun = value;
        }

        public void AddHitStun(int value)
        {
            HitStun += value;
        }

        public void SetChargeLevel(int value)
        {
            CurrentChargeLevel = value;
        }

        public void SetChargeLevelCharge(int value)
        {
            CurrentChargeLevelCharge = value;
        }

        public void IncrementChargeLevelCharge(int maxCharge)
        {
            CurrentChargeLevelCharge = Mathf.Clamp(CurrentChargeLevelCharge+1, 0, maxCharge);
        }

        public int GetTeam()
        {
            return team;
        }
    }
}