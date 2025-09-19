using UnityEngine;

[DisallowMultipleComponent]
public class Clock : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _timeModifier = 0f;

    private int _stopCount = 0;
    private float _prevScale = 1f;

    /// <summary>
    /// stop == true, request pause/slow
    /// stop == false, release one request
    /// </summary>
    public void StopTime(bool stop = true)
    {
        int prev = _stopCount;
        _stopCount += stop ? 1 : -1;
        if (_stopCount < 0) _stopCount = 0;

        if (prev == 0 && _stopCount == 1)
        {
            _prevScale = Time.timeScale;
            Time.timeScale = _timeModifier;
        }
        else if (prev == 1 && _stopCount == 0)
        {
            Time.timeScale = _prevScale;
        }
    }

    private void OnDisable()
    {
        // don't leave time frozen if clock gets destroyed
        if (_stopCount > 0)
        {
            Time.timeScale = _prevScale;
            _stopCount = 0;
        }
    }
}
