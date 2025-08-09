using UnityEditor;
using UnityEngine;

public class TimeScaleEditor : EditorWindow
{
    private const float TransitionDuration = 0.5f; // Smooth time scale transition during play mode
    private const float ButtonHeight = 35f;
    private const float ButtonHorizontalSpacing = 4f;

    private float _targetTimeScale = 1.0f;
    private float _currentTimeScale = 1.0f;
    private float _timeScaleOnTransitionStart;
    private double _transitionStartTime;

    private int _targetFrameRate = 60;
    private int _currentFrameRate = 60;

    [MenuItem("Tools/Time Scale Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<TimeScaleEditor>("Time Scale");
        window.minSize = new Vector2(250, 210);
        window.Show();
    }

    private void OnEnable()
    {
        _targetTimeScale = Time.timeScale;
        _currentTimeScale = Time.timeScale;
        _timeScaleOnTransitionStart = Time.timeScale;

        _targetFrameRate = Application.targetFrameRate;
        _currentFrameRate = Application.targetFrameRate;
    }

    private void OnGUI()
    {
        DrawTimeScaleControls();
        EditorGUILayout.Space(10);
        DrawFrameRateControls();
    }

    private void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            _currentTimeScale = Time.timeScale;
            _currentFrameRate = Application.targetFrameRate;
            Repaint();
            return;
        }

        // Time Scale Smooth Transition
        float elapsed = (float)(EditorApplication.timeSinceStartup - _transitionStartTime);
        float t = Mathf.Clamp01(elapsed / TransitionDuration);

        _currentTimeScale = Mathf.Lerp(_timeScaleOnTransitionStart, _targetTimeScale, t);
        Time.timeScale = _currentTimeScale;

        if (Application.targetFrameRate != _targetFrameRate)
        {
            Application.targetFrameRate = _targetFrameRate;
        }
        _currentFrameRate = Application.targetFrameRate;

        Repaint();
    }

    #region UI Drawing Methods

    private void DrawTimeScaleControls()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Current Time Scale:");
        EditorGUILayout.LabelField($"{_currentTimeScale:F2}", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        _targetTimeScale = EditorGUILayout.Slider("Target", _targetTimeScale, 0.0f, 2.0f);
        if (EditorGUI.EndChangeCheck())
        {
            SetTargetTimeScale(_targetTimeScale);
        }

        EditorGUILayout.Space(2);

        // Preset Buttons
        float buttonWidth = (position.width / 5) - ButtonHorizontalSpacing;
        EditorGUILayout.BeginHorizontal();
        CreateTimeScalePresetButton("0x", "PauseButton", 0.0f, buttonWidth);
        CreateTimeScalePresetButton("0.1x", "DotFrameDotted", 0.1f, buttonWidth);
        CreateTimeScalePresetButton("0.3x", "StepButton", 0.3f, buttonWidth);
        CreateTimeScalePresetButton("1x", "PlayButton", 1.0f, buttonWidth);
        CreateTimeScalePresetButton("2x", "Animation.LastKey", 2.0f, buttonWidth);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawFrameRateControls()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Current Frame Rate:");
        string currentFpsText = _currentFrameRate == -1 ? "Uncapped" : _currentFrameRate.ToString();
        EditorGUILayout.LabelField($"{currentFpsText}", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        _targetFrameRate = EditorGUILayout.IntSlider("Target", _targetFrameRate, -1, 120);
        if (EditorGUI.EndChangeCheck())
        {
            SetTargetFrameRate(_targetFrameRate);
        }

        EditorGUILayout.Space(2);

        // Preset Buttons
        float buttonWidth = (position.width / 5) - ButtonHorizontalSpacing;
        EditorGUILayout.BeginHorizontal();
        CreateFrameRatePresetButton("UNCAP", -1, buttonWidth);
        CreateFrameRatePresetButton("5", 5, buttonWidth);
        CreateFrameRatePresetButton("30", 30, buttonWidth);
        CreateFrameRatePresetButton("60", 60, buttonWidth);
        CreateFrameRatePresetButton("120", 120, buttonWidth);
        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region Helper Methods

    private void CreateTimeScalePresetButton(string label, string iconName, float value, float width)
    {
        var content = new GUIContent($" {label}", EditorGUIUtility.IconContent(iconName).image);
        if (GUILayout.Button(content, GUILayout.Width(width), GUILayout.Height(ButtonHeight)))
        {
            SetTargetTimeScale(value);
        }
    }

    private void CreateFrameRatePresetButton(string label, int value, float width)
    {
        if (GUILayout.Button(label, GUILayout.Width(width), GUILayout.Height(ButtonHeight)))
        {
            SetTargetFrameRate(value);
        }
    }

    private void SetTargetTimeScale(float newTarget)
    {
        _targetTimeScale = newTarget;
        _timeScaleOnTransitionStart = _currentTimeScale;
        _transitionStartTime = EditorApplication.timeSinceStartup;

        // If we're not in play mode, apply the change immediately.
        if (!EditorApplication.isPlaying)
        {
            Time.timeScale = _targetTimeScale;
        }
    }

    private void SetTargetFrameRate(int newTarget)
    {
        _targetFrameRate = newTarget;

        // If we're not in play mode, apply the change immediately.
        if (!EditorApplication.isPlaying)
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }

    #endregion
}