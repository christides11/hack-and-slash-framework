namespace HnSF.Fighters
{
    public class FighterStateBase
    {
        /// <summary>
        /// Get the name of the state.
        /// </summary>
        /// <returns>The state name.</returns>
        public virtual string GetName()
        {
            return "State";
        }

        /// <summary>
        /// Called when the state is changed to.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Called every update tick.
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// Called every late update tick.
        /// </summary>
        public virtual void OnLateUpdate()
        {

        }

        /// <summary>
        /// Called during OnUpdate to check if we should transition into another state.
        /// </summary>
        /// <returns>Returns true if we transitioned into another state.</returns>
        public virtual bool CheckInterrupt()
        {
            return false;
        }

        /// <summary>
        /// Called when we transition into another state.
        /// </summary>
        public virtual void OnInterrupted()
        {

        }
    }
}
