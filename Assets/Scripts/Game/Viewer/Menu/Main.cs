using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : Menu
{
    [SerializeField] private string _game;

    public void NewGame()
    {
        SceneManager.LoadScene(_game);
    }
}
