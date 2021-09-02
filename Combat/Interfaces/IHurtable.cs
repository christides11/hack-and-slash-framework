namespace HnSF.Combat
{
    public interface IHurtable
    {
        int GetTeam();
        HitReactionBase Hurt(HurtInfoBase hurtInfo);
    }
}