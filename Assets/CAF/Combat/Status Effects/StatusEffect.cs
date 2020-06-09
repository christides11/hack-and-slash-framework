using CAF.Entities;

namespace CAF.Combat
{
    public class StatusEffect
    {
        protected int time;

        public virtual int GetTime()
        {
            return time;
        }

        public virtual void SetTime(int time)
        {
            this.time = time;
        }

        public virtual string GetName()
        {
            return "";
        }

        public virtual void Initialize(EntityController entity)
        {

        }

        public virtual void Tick(EntityController entity)
        {
            time -= 1;
        }

        public virtual void OnRemoved(EntityController entity)
        {

        }
    }
}