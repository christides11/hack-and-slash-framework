namespace HnSF.Combat
{
    public interface IHealable
    {
        int GetTeam();
        void Heal(HealInfoBase healInfo);
    }
}