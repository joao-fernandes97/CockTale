using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : Menu
{
    [SerializeField] private SceneAsset _Menu;
    [SerializeField] private Clock _clock;

    private void OnEnable()
    {
        _clock.StopTime(true);
    }
    private void OnDisable()
    {
        _clock.StopTime(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(_Menu.name);
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
