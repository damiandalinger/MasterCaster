/// <summary>
/// Interface for tabs that contain animations or effects that may need to finish before allowing user progression.
/// </summary>

/// <remarks>
/// 27/06/2025 by Damian Dalinger: Interface creation for animation interruption support.
/// </remarks>

public interface ITabInterruptible
{
    // Gets whether this tab is currently busy with an unfinished animation or visual process.
    bool IsBusy { get; }

    // Immediately completes any running animation or effect and jumps to its final visual state.
    void SkipToEnd();
}
