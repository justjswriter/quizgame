using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject mainMenuUI;
    public GameObject levelSelectionPanel;
    public GameObject level1Button;
    public GameObject level2Button;
    public GameObject level3Button;

    void Start()
    {
        SetMainMenuActive(true);
        SetInstructionsPanelActive(false);
        SetLevelSelectionPanelActive(false);
    }

    public void StartGame()
    {
        SetMainMenuActive(false);
        SetLevelSelectionPanelActive(true);
    }

    public void ShowInstructions()
    {
        Debug.Log("Instructions button clicked!");
        SetMainMenuActive(false);
        SetInstructionsPanelActive(true);
    }

    public void HideInstructions()
    {
        SetMainMenuActive(true);
        SetInstructionsPanelActive(false);
    }

    public void SelectLevel1()
    {
        PlayerPrefs.SetInt("SelectedLevel", 1);
        LoadGameScene();
    }

    public void SelectLevel2()
    {
        PlayerPrefs.SetInt("SelectedLevel", 2);
        LoadGameScene();
    }

    public void SelectLevel3()
    {
        PlayerPrefs.SetInt("SelectedLevel", 3);
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void SetMainMenuActive(bool isActive)
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(isActive);
        }
    }

    private void SetInstructionsPanelActive(bool isActive)
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(isActive);
            Debug.Log("Instructions Panel is now " + (isActive ? "visible" : "hidden"));
        }
    }

    private void SetLevelSelectionPanelActive(bool isActive)
    {
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(isActive);
            Debug.Log("Level Selection Panel is now " + (isActive ? "visible" : "hidden"));
        }
    }
}
