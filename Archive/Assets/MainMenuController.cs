using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // References to the UI elements
    public GameObject instructionsPanel;      // Instructions Panel
    public GameObject mainMenuUI;             // Main Menu UI (Title, Start, Instructions buttons)
    public GameObject levelSelectionPanel;   // Level Selection Panel
    public GameObject level1Button;           // Level 1 Button
    public GameObject level2Button;           // Level 2 Button
    public GameObject level3Button;           // Level 3 Button

    void Start()
    {
        // Hide instructions and level selection panels, show the main menu at the start
        SetMainMenuActive(true);
        SetInstructionsPanelActive(false);
        SetLevelSelectionPanelActive(false);
    }

    // Start the game and load the selected level
    public void StartGame()
    {
        // Show the level selection panel
        SetMainMenuActive(false);
        SetLevelSelectionPanelActive(true);
    }

    // Show instructions when the instructions button is clicked
    public void ShowInstructions()
    {
        Debug.Log("Instructions button clicked!");
        SetMainMenuActive(false);
        SetInstructionsPanelActive(true);
    }

    // Hide instructions and go back to the main menu
    public void HideInstructions()
    {
        SetMainMenuActive(true);
        SetInstructionsPanelActive(false);
    }

    // Select Level 1 and load the corresponding game scene
    public void SelectLevel1()
    {
        PlayerPrefs.SetInt("SelectedLevel", 1);
        LoadGameScene();
    }

    // Select Level 2 and load the corresponding game scene
    public void SelectLevel2()
    {
        PlayerPrefs.SetInt("SelectedLevel", 2);
        LoadGameScene();
    }

    // Select Level 3 and load the corresponding game scene
    public void SelectLevel3()
    {
        PlayerPrefs.SetInt("SelectedLevel", 3);
        LoadGameScene();
    }

    // Load the game scene based on the selected level
    private void LoadGameScene()
    {
        // Make sure you have a scene named "GameScene" or load it dynamically using level-specific scene names
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with the actual scene name if needed
    }

    // Helper method to set the Main Menu UI visibility
    private void SetMainMenuActive(bool isActive)
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(isActive);
        }
    }

    // Helper method to set the Instructions Panel visibility
    private void SetInstructionsPanelActive(bool isActive)
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(isActive);
            Debug.Log("Instructions Panel is now " + (isActive ? "visible" : "hidden"));
        }
    }

    // Helper method to set the Level Selection Panel visibility
    private void SetLevelSelectionPanelActive(bool isActive)
    {
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(isActive);
            Debug.Log("Level Selection Panel is now " + (isActive ? "visible" : "hidden"));
        }
    }
}
