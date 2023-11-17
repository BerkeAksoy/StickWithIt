using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScroll : MonoBehaviour
{
    private GameManager gm;
    private GameObject unlockBallsButton, unlockSpecificButton;
    private Text unlocksAt, explanation, ballName;
    public Button[] ballButtons;
    public BallSO[] ballDatas;
    private SwipeMenu swipe;
    private int curIndex = 0;

    private void Start()
    {
        gm = GameManager.Instance;
        swipe = GetComponent<SwipeMenu>();
        unlockBallsButton = GameObject.Find("Canvas/Ball_Scroll/Unlock_Balls_Button");
        unlockSpecificButton = GameObject.Find("Canvas/Ball_Scroll/Unlock");
        unlocksAt = GameObject.Find("Canvas/Ball_Scroll/Unlock/At").GetComponent<Text>();
        explanation = GameObject.Find("Canvas/Ball_Scroll/Ball_Explanation").GetComponent<Text>();
        ballName = GameObject.Find("Canvas/Ball_Scroll/Ball_Name").GetComponent<Text>();

        if (gm.AllBallsUnlocked)
        {
            unlockBallsButton.SetActive(false);
            unlockSpecificButton.SetActive(false);
        }

        curIndex = swipe.index;
        checkBallLock();
        MakeInteractable();
    }

    private void Update()
    {
        if (swipe.index != curIndex)
        {
            curIndex = swipe.index;
            checkBallLock();
            MakeInteractable();
        }
    }

    private void checkBallLock()
    {
        if (!gm.UnlockedBalls[curIndex])
        {
            unlockSpecificButton.SetActive(true);
            unlocksAt.text = ballDatas[curIndex].unlocksAt;
        }
        else
        {
            unlockSpecificButton.SetActive(false);
            unlocksAt.text = "";
        }
    }

    private void MakeInteractable()
    {
        for (int i = 0; i < ballButtons.Length; i++)
        {
            if (i == curIndex)
            {
                if (gm.unlockedBalls[i])
                {
                    ballButtons[i].interactable = true;
                }

                explanation.text = ballDatas[i].explanation;
                ballName.text = ballDatas[i].name;
            }
            else
            {
                ballButtons[i].interactable = false;
            }
        }
    }

    public void UnlockAllBalls()
    {
        // Satın alma ekranı
    }

    public void UnlockBall()
    {
        // Satın alma ekranı
    }

    public void SelectBall(int index)
    {

    }


}
