/// <summary>
/// Defines save and load behavior for ScriptableObjects or runtime components.
/// </summary>
/// <remarks>
/// 30/05/2025 by Damian Dalinger: Script creation.
/// </remarks>

namespace ProjectCeros
{

    public interface ISaveable
    {
        string SaveKey { get; }
        object CaptureState();
        void RestoreState(object state);
    }
}