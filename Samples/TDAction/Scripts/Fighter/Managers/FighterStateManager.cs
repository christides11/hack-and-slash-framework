using HnSF.Fighters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDAction.Fighter
{
    public class FighterStateManager : HnSF.Fighters.FighterStateManager
    {
        public override IFighterBase Manager { get { return manager; } }

        [SerializeField] protected FighterManager manager;

        public override void AddState(FighterStateBase state, ushort stateNumber)
        {
            (state as FighterState).Manager = manager;
            base.AddState(state, stateNumber);
        }
    }
}