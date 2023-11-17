using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    private Ball ball;
    private GameObject pausePanel, deadPanel, undoPanel;
    private Button undoButton, pauseButton;
    private GameManager gm;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("UI Manager is null.");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private Text jumpsLeftText;

    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();

        gm = GameManager.Instance;
        pausePanel = GameObject.Find("Canvas/Pause_Panel");
        deadPanel = GameObject.Find("Canvas/Dead_Panel");
        undoPanel = GameObject.Find("Canvas/Undo_Panel");
        undoButton = GameObject.Find("Canvas/Undo_Button").GetComponent<Button>();
        pauseButton = GameObject.Find("Canvas/Pause_button").GetComponent<Button>();
        jumpsLeftText = GameObject.Find("Canvas/Jumps_Left").GetComponent<Text>();
        UpdateJumpLeftCount(GameManager.Instance.JumpCount);

        pausePanel.SetActive(false);
        deadPanel.SetActive(false);
        undoPanel.SetActive(false);
        DisableUndoButton();
    }

    public void UpdateJumpLeftCount(int count)
    {
        jumpsLeftText.text = "Jumps\nLeft: " + count;
    }

    public void OpenUndoPanel()
    {
        if (ball.CanUndo && ball.CanMove)
        {
            Time.timeScale = 0;
            gm.Paused = true;
            DisableUndoButton();
            pauseButton.interactable = false;
            undoPanel.SetActive(true);
        }
    }

    public void CancelUndoPanel()
    {
        Time.timeScale = 1;
        gm.Paused = false;
        EnableUndoButton();
        pauseButton.interactable = true;
        undoPanel.SetActive(false);
    }

    public void UndoEvent()
    {
        //Reward
        ball.UndoMove();

        Time.timeScale = 1;
        gm.Paused = false;
        pauseButton.interactable = true;
        undoPanel.SetActive(false);
    }

    public void ForceUndoButton()
    {
        // After a video
        deadPanel.SetActive(false);
        pauseButton.interactable = true;
        ball.UndoMove();
        Time.timeScale = 1;
        ball.Died = false;
    }

    public void EnableUndoButton()
    {
        undoButton.GetComponentInChildren<Text>().enabled = true;
        undoButton.interactable = true;
    }

    public void DisableUndoButton()
    {
        undoButton.GetComponentInChildren<Text>().enabled = false;
        undoButton.interactable = false;
    }

    public void PauseEvent()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        gm.Paused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        gm.Paused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        LevelManager.Instance.Restart();
        gm.ResetJumpCount();
        gm.Paused = false;
        ball.Died = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        LevelManager.Instance.LoadScene(0);
        gm.Paused = false;
    }

    public void YouDied()
    {
        Time.timeScale = 0;
        DisableUndoButton();
        pauseButton.interactable = false;
        deadPanel.SetActive(true);
    }
}
