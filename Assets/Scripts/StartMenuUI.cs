using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{

    private static StartMenuUI instance;

    private Button backButton;
    private GameObject mainMenu;
    private GameObject levelScroll, ballScroll, settingsPart;
    private Slider volumeSlider;
    private Text difText;
    private int continueLevel = 1;
    private GameManager gm;
    public AudioMixer mixer;

    public static StartMenuUI Instance
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

    private void Start()
    {
        gm = GameManager.Instance;

        continueLevel = gm.SavedLevel;

        if(continueLevel == 1)
        {
            GameObject.Find("Canvas/MainMenu/Button_Layout/Continue").GetComponentInChildren<Text>().text = "Start Game";
        }
        else
        {
            GameObject.Find("Canvas/MainMenu/Button_Layout/Continue").GetComponentInChildren<Text>().text = "Continue Level " + continueLevel;
        }

        backButton = GameObject.Find("Canvas/Back_Button").GetComponent<Button>();
        mainMenu = GameObject.Find("Canvas/MainMenu");
        levelScroll = GameObject.Find("Canvas/Level_Scroll");
        ballScroll = GameObject.Find("Canvas/Ball_Scroll");
        settingsPart = GameObject.Find("Canvas/Settings_Part");
        difText = GameObject.Find("Canvas/Settings_Part/Difficulty_Button/Dif_Text").GetComponent<Text>();
        volumeSlider = GameObject.Find("Canvas/Settings_Part/Music_Slider").GetComponent<Slider>();

        backButton.interactable = false;
        levelScroll.SetActive(false);
        ballScroll.SetActive(false);
        settingsPart.SetActive(false);

        setVolume(gm.Volume);
    }

    public void ContinueButton()
    {
        LevelScroll ls = levelScroll.GetComponentInChildren<LevelScroll>();

        switch (gm.getDifficulty())
        {
            case 0:
                gm.CurrentMaxAllowedJump = ls.levelDatas[continueLevel - 1].allowedJumpCount[0];
                gm.ResetJumpCount();
                break;
            case 1:
                gm.CurrentMaxAllowedJump = ls.levelDatas[continueLevel - 1].allowedJumpCount[1];
                gm.ResetJumpCount();
                break;
            case 2:
                gm.CurrentMaxAllowedJump = ls.levelDatas[continueLevel - 1].allowedJumpCount[2];
                gm.ResetJumpCount();
                break;
            default:
                gm.setDifficulty(0);
                gm.CurrentMaxAllowedJump = ls.levelDatas[0].allowedJumpCount[0];
                gm.ResetJumpCount();
                break;
        }

        LevelManager.Instance.LoadScene(continueLevel);
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        backButton.interactable = false;

        if (ballScroll.activeInHierarchy)
        {
            ballScroll.SetActive(false);
        }

        if (levelScroll.activeInHierarchy)
        {
            levelScroll.SetActive(false);
        }

        if (settingsPart.activeInHierarchy)
        {
            settingsPart.SetActive(false);
        }

        gm.Save();
    }

    public void SelectBallButton()
    {
        mainMenu.SetActive(false);
        backButton.interactable = true;

        ballScroll.SetActive(true);
    }

    public void SelectLevelButton()
    {
        mainMenu.SetActive(false);
        backButton.interactable = true;

        levelScroll.SetActive(true);
    }

    public void SettingsButton()
    {
        backButton.interactable = true;
        mainMenu.SetActive(false);

        settingsPart.SetActive(true);
        switch (gm.getDifficulty())
        {
            case 0:
                difText.text = "Easy";
                break;
            case 1:
                difText.text = "Medium";
                break;
            case 2:
                difText.text = "Hard";
                break;
        }
        volumeSlider.value = gm.Volume;
    }

    public void ChangeDifficulty()
    {
        switch (gm.getDifficulty())
        {
            case 0:
                difText.text = "Medium";
                gm.setDifficulty(1);
                break;
            case 1:
                difText.text = "Hard";
                gm.setDifficulty(2);
                break;
            case 2:
                difText.text = "Easy";
                gm.setDifficulty(0);
                break;
        }
    }

    public void setVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        gm.Volume = sliderValue;
    }

}
