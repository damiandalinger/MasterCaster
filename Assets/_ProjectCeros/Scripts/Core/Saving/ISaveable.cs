namespace ProjectCeros
{

    public interface ISaveable
    {
        string SaveKey { get; }
        object CaptureState();
        void RestoreState(object state);
    }
}