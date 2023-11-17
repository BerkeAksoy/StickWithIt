using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScroll : MonoBehaviour
{

    private GameManager gm;
    private GameObject unlockLevelsButton;
    private SwipeMenu swipe;
    public Button[] levelButtons;
    public Sprite lockSprite, normalSprite;
    public LevelSO[] levelDatas;

    private int curIndex = 0;

    private void Start()
    {
        gm = GameManager.Instance;
        swipe = GetComponent<SwipeMenu>();
        unlockLevelsButton = GameObject.Find("Canvas/Level_Scroll/Unlock_Levels_Button");

        if (gm.AllLevelsUnlocked)
        {
            unlockLevelsButton.SetActive(false);
        }
        
        CheckLevelLocks(gm.unlockedLevels);

        curIndex = swipe.index;
        MakeInteractable();
    }

    private void Update()
    {
        if(curIndex != swipe.index)
        {
            curIndex = swipe.index;
            MakeInteractable();
        }
    }

    private void CheckLevelLocks(bool[] levelsUnlocked)
    {
        for (int i = 0; i < levelsUnlocked.Length; i++)
        {
            if (!levelsUnlocked[i])
            {
                levelButtons[i].image.sprite = lockSprite;
            }
            else
            {
                levelButtons[i].image.sprite = normalSprite;
            }
        }
    }

    private void MakeInteractable()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i == curIndex)
            {
                if (gm.unlockedLevels[i])
                {
                    levelButtons[i].interactable = true;
                }
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void UnlockAllLevels()
    {
        // Satın alma ekranı
    }

    public void SelectLevel(int index)
    {
        LevelManager.Instance.LoadScene(index);
        
        switch (gm.getDifficulty())
        {
            case 0:
                gm.CurrentMaxAllowedJump = levelDatas[index - 1].allowedJumpCount[0];
                gm.ResetJumpCount();
                break;
            case 1:
                gm.CurrentMaxAllowedJump = levelDatas[index - 1].allowedJumpCount[1];
                gm.ResetJumpCount();
                break;
            case 2:
                gm.CurrentMaxAllowedJump = levelDatas[index - 1].allowedJumpCount[2];
                gm.ResetJumpCount();
                break;
            default:
                gm.setDifficulty(0);
                gm.CurrentMaxAllowedJump = levelDatas[0].allowedJumpCount[0];
                gm.ResetJumpCount();
                break;
        }
    }


}
