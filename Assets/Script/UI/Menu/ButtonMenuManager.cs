using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private GameObject panelOption;
    [SerializeField] private Button exitButton;

    [Header("InGameMenu")]
    [SerializeField] private GameObject panelInGameOption;
    [SerializeField] private TMP_Text gameoverText;
    [SerializeField] private TMP_Text tryAgainText;

    private bool isTryAgain;

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Option()
    {
        panelOption.SetActive(true);
        if(playButton != null && optionButton != null && exitButton != null)
        {
            playButton.interactable = false;
            optionButton.interactable = false;
            exitButton.interactable = false;
        }
    }

    public void ExitOption()
    {
        panelOption.SetActive(false);
        if (playButton != null && optionButton != null && exitButton != null)
        {
            playButton.interactable = true;
            optionButton.interactable = true;
            exitButton.interactable = true;
        }
    }

    public void TryAgain()
    {
        if(isTryAgain)
        {
            Play();
        }
        else
        {
            panelInGameOption.SetActive(false);
        }
    }

    public void ShowGameOver(bool isGameOver)
    {
        if (isGameOver)
        {
            gameoverText.text = "Game Over";
            tryAgainText.text = "Try Again";
            isTryAgain = true;
        }
        else
        {
            gameoverText.text = "Congratulation On Finishing The Game";
            tryAgainText.text = "Keep Exploring";
            isTryAgain = false;
        }
        panelInGameOption.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
