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
        public int HitStop { get; } = 0;
        public int HitStun { get; } = 0;
        public int CurrentChargeLevel { get; } = 0;
        public int CurrentChargeLevelCharge { get; } = 0;

        public void Cleanup()
        {
            throw new System.NotImplementedException();
        }

        public bool CheckForInputSequence(InputSequence sequence, uint baseOffset = 0, bool processSequenceButtons = false,
            bool holdInput = false)
        {
            throw new System.NotImplementedException();
        }

        public void SetHitStop(int value)
        {
            throw new System.NotImplementedException();
        }

        public void AddHitStop(int value)
        {
            throw new System.NotImplementedException();
        }

        public void SetHitStun(int value)
        {
            throw new System.NotImplementedException();
        }

        public void AddHitStun(int value)
        {
            throw new System.NotImplementedException();
        }

        public void SetChargeLevel(int value)
        {
            throw new System.NotImplementedException();
        }

        public void SetChargeLevelCharge(int value)
        {
            throw new System.NotImplementedException();
        }

        public void IncrementChargeLevelCharge(int maxCharge)
        {
            throw new System.NotImplementedException();
        }

        public int GetTeam()
        {
            throw new System.NotImplementedException();
        }
    }
}