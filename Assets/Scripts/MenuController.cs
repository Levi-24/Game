using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Enum to represent the different menu states.
    enum MenuState
    {
        Menu,      // Main menu
        Settings,  // Settings menu
        Credits    // Credits menu
    }

    [SerializeField] private GameObject menu;       // Reference to the main menu GameObject.
    [SerializeField] private GameObject settings;    // Reference to the settings menu GameObject.
    [SerializeField] private GameObject credits;     // Reference to the credits menu GameObject.

    private bool isFullScreen = false;               // Indicates whether the game is in fullscreen mode.
    private bool isMuted = false;                    // Indicates whether the audio is muted.

    private void Start()
    {
        // Check if any menu references are missing and log an error if so.
        if (menu == null || settings == null || credits == null)
        {
            Debug.LogError("MenuController: One or more menu references are missing.");
        }

        OpenMenu(); // Open the main menu at the start of the game.
    }

    // Method to start the game and load the GameMap scene.
    public void StartGame()
    {
        // Check if the GameMap scene can be loaded.
        if (Application.CanStreamedLevelBeLoaded("GameMap"))
        {
            SceneManager.LoadScene("GameMap"); // Load the GameMap scene.
        }
        else
        {
            Debug.LogError("MenuController: GameMap scene is not available."); // Log an error if the scene cannot be loaded.
        }
    }

    // Method to open the settings menu.
    public void OpenSettings()
    {
        SetActiveMenu(MenuState.Settings); // Set the active menu to settings.
    }

    // Method to open the credits menu.
    public void OpenCredits()
    {
        SetActiveMenu(MenuState.Credits); // Set the active menu to credits.
    }

    // Method to open the main menu.
    public void OpenMenu()
    {
        SetActiveMenu(MenuState.Menu); // Set the active menu to the main menu.
    }

    // Method to toggle fullscreen mode.
    public void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen; // Toggle the fullscreen state.
        Screen.fullScreen = isFullScreen; // Apply the fullscreen setting.
    }

    // Method to toggle audio mute state.
    public void ToggleMute()
    {
        isMuted = !isMuted; // Toggle the mute state.
        AudioListener.pause = isMuted; // Apply the mute setting.
    }

    // Method to exit the game.
    public void ExitGame()
    {
        Application.Quit(); // Quit the application.
    }

    // Method to set the active menu based on the provided state.
    private void SetActiveMenu(MenuState state)
    {
        // Ensure menu references are not null.
        if (menu == null || settings == null || credits == null) return;

        // Activate the appropriate menu based on the current state.
        menu.SetActive(state == MenuState.Menu);
        settings.SetActive(state == MenuState.Settings);
        credits.SetActive(state == MenuState.Credits);
    }
}
