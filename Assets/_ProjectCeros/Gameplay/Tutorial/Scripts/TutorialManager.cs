/// <summary>
/// Central controller for showing tutorial steps by ID, tracking completion, setting the finished flag, and invoking an event when all steps are done.
/// </summary>

/// <remarks>
/// 11/08/2025 by Damian Dalinger: Script creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCeros
{
    public class TutorialManager : MonoBehaviour
    {
        #region Fields

        [Tooltip("All relevant tutorial steps.")]
        [SerializeField] private List<TutorialStep> _steps = new List<TutorialStep>();

        [Tooltip("Panel placed in the scene and initially disabled.")]
        [SerializeField] private TutorialPanel _panelInstance;

        [Tooltip("Global finished flag stored via BoolReference.")]
        [SerializeField] private BoolReference _isTutorialFinished;

        [Tooltip("Invoked exactly once when all tutorial steps are completed.")]
        [SerializeField] private UnityEvent _onAllStepsCompleted = new UnityEvent();

        private TutorialStep _currentStep;

        #endregion

        #region LifeCycle Methods

        private void Awake()
        {
            ResetAllSteps();
        }

        #endregion

        #region Public Methods

        // Triggers the tutorial step for the given ID if not completed yet.
        public void TriggerStep(int id)
        {
            // Do not show anything if the tutorial is already finished.
            if (_isTutorialFinished != null && _isTutorialFinished.Value)
                return;

            var step = FindStepById(id);
            if (step == null)
            {
                Debug.LogWarning($"TutorialManager.TriggerStep: No step for Id {id} found.");
                return;
            }

            if (step.IsComplete)
                return;

            ShowStep(step);
        }

        #endregion

        #region Private Methods

        // Returns true if all configured steps are completed.
        private bool AllStepsCompleted()
        {
            for (int i = 0; i < _steps.Count; i++)
            {
                var s = _steps[i];
                if (s == null) continue;
                if (!s.IsComplete)
                    return false;
            }
            return true;
        }

        // Marks a step complete and finalizes when all steps are done.
        private void CompleteStep(TutorialStep step)
        {
            step.IsComplete = true;
            HidePanel();

            if (AllStepsCompleted())
            {
                _isTutorialFinished.Variable.SetValue(true);
                _onAllStepsCompleted?.Invoke();
            }
        }

        // Finds the TutorialStep ScriptableObject by searching for the id.
        private TutorialStep FindStepById(int id)
        {
            for (int i = 0; i < _steps.Count; i++)
            {
                var s = _steps[i];
                if (s != null && s.Id == id)
                    return s;
            }
            return null;
        }

        // Hides the panel instance.
        private void HidePanel()
        {
            if (_panelInstance != null && _panelInstance.gameObject.activeSelf)
                _panelInstance.gameObject.SetActive(false);
        }

        // Button handler for the panel's "Understood" action.
        private void OnUnderstoodClicked()
        {
            if (_currentStep != null)
            {
                CompleteStep(_currentStep);
                _currentStep = null;
            }
        }

        // Clears completion and resets the finished flag at scene start.
        private void ResetAllSteps()
        {
            for (int i = 0; i < _steps.Count; i++)
            {
                var s = _steps[i];
                if (s == null) continue;
                s.IsComplete = false;
            }
        }

        // Shows the scene-placed panel and binds the current step content.
        private void ShowStep(TutorialStep step)
        {
            _currentStep = step;
            _panelInstance.Bind(step.Headline, step.Text, OnUnderstoodClicked);
            _panelInstance.gameObject.SetActive(true);
        }

        #endregion
    }
}