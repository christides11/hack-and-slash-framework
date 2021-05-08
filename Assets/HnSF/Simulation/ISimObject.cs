namespace HnSF.Simulation
{
    /// <summary>
    /// A SimObject should be implemented by any class that will be apart of the simulation.
    /// </summary>
    public interface ISimObject
    {
        /// <summary>
        /// Called every simulations tick.
        /// </summary>
        void SimUpdate();
        /// <summary>
        /// Called every simulation tick after all updates are called.
        /// </summary>
        void SimLateUpdate();
    }
}