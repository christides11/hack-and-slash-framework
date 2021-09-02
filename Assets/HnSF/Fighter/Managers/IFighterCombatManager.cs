using HnSF.Combat;
using HnSF.Input;

namespace HnSF.Fighters
{
    public interface IFighterCombatManager
    {
        int HitStun { get; }
        int HitStop { get; }
        int CurrentChargeLevel { get; }
        int CurrentChargeLevelCharge { get; }
        /// <summary>
        /// The current moveset we are assigned.
        /// </summary>
        MovesetDefinition CurrentMoveset { get; }
        /// <summary>
        /// The identifier of the current moveset.
        /// </summary>
        int CurrentMovesetIdentifier { get; }
        /// <summary>
        /// The moveset that the current attack belongs to. Not the same as our current moveset.
        /// </summary>
        MovesetDefinition CurrentAttackMoveset { get; }
        /// <summary>
        /// The identifier of the moveset that the current attack belongs to. Not the same as our current moveset.
        /// </summary>
        int CurrentAttackMovesetIdentifier { get; }
        /// <summary>
        /// The attack node of the current attack.
        /// </summary>
        MovesetAttackNode CurrentAttackNode { get; }
        int CurrentAttackNodeIdentifier { get; }

        void CLateUpdate();
        int GetMovesetCount();
        MovesetDefinition GetMoveset(int index);
        void Cleanup();
        void SetAttack(int attackNodeIdentifier);
        void SetAttack(int attackNodeIdentifier, int attackMovesetIdentifier);
        int TryAttack();
        int TryCancelList(int cancelListID);
        bool CheckForInputSequence(InputSequence sequence, uint baseOffset = 0, bool processSequenceButtons = false, bool holdInput = false);

        void SetHitStop(int value);
        void AddHitStop(int value);
        void SetHitStun(int value);
        void AddHitStun(int value);
        void SetMoveset(int movesetIndex);
        void SetChargeLevel(int value);
        void SetChargeLevelCharge(int value);
        void IncrementChargeLevelCharge(int maxCharge);
        int GetTeam();
    }
}