using HnSF.Input;

namespace HnSF.Fighters
{
    public interface IFighterCombatManager
    {
        int HitStun { get; }
        int HitStop { get; }
        int CurrentChargeLevel { get; }
        int CurrentChargeLevelCharge { get; }
        
        void Cleanup();

        void SetHitStun(int value);
        void AddHitStun(int value);
        void SetHitStop(int value);
        void AddHitStop(int value);
        void SetChargeLevel(int value);
        void SetChargeLevelCharge(int value);
        void IncrementChargeLevelCharge(int maxCharge);
    }
}