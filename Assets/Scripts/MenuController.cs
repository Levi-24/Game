using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;

    private bool isFullScreen = false;
    private bool isMuted = false;

    private void Start()
    {
        if (menu == null || settings == null || credits == null)
        {
            Debug.LogError("MenuController: One or more menu references are missing.");
        }

        OpenMenu();
    }

    public void StartGame()
    {
        if (Application.CanStreamedLevelBeLoaded("GameMap"))
        {
            SceneManager.LoadScene("GameMap");
        }
        else
        {
            Debug.LogError("MenuController: GameMap scene is not available.");
        }
    }

    public void OpenSettings()
    {
        SetActiveMenu(MenuState.Settings);
    }

    public void OpenCredits()
    {
        SetActiveMenu(MenuState.Credits);
    }

    public void OpenMenu()
    {
        SetActiveMenu(MenuState.Menu);
    }

    public void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private enum MenuState
    {
        Menu,
        Settings,
        Credits
    }

    private void SetActiveMenu(MenuState state)
    {
        if (menu == null || settings == null || credits == null) return;

        menu.SetActive(state == MenuState.Menu);
        settings.SetActive(state == MenuState.Settings);
        credits.SetActive(state == MenuState.Credits);
    }
}
