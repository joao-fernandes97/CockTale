using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : Menu
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private Clock _clock;

    private void Start()
    {
        Continue();
    }

    private void Update()
    {
        if (  SceneManager.GetActiveScene().name != _mainMenu && InputManager.Pause() )
        {
            if ( _pause.activeSelf )
                Continue();
            else
            {
                _pause.SetActive(true);
                _clock.StopTime(true);
            }
        }
        // Debug.Log("time scale?" + Time.timeScale);
    }

    public void Main()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    public override void Continue()
    {
        _clock.StopTime(false);
        // InputManager.Paused = false;

        _settings.TurnOffSettings();
        _pause.SetActive(false);

        // Debug.Log("unloading");
    }

    public void OnDestroy()
    {
        Debug.Log("Destroying pause");
        Continue();        
    }
}
