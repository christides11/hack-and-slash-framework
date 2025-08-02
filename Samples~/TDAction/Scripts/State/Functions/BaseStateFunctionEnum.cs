namespace HnSF.Sample.TDAction.State
{
    [System.Serializable]
    public enum StateFunctionEnum : int
    {
        NULL = 0,
        CHANGE_STATE,
        APPLY_GRAVITY,
        APPLY_TRACTION,
        SET_FALL_SPEED
    }
}