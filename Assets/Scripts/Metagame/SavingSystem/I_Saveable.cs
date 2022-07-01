public interface I_Saveable
{
    object CaptureState();
    void RestoreState(object state);
}