using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    private bool paused = false;
    private int difLevel;
    private int savedLevel = 1;
    private float volume = 1;
    public bool[] unlockedBalls, unlockedLevels;
    private bool allBallsUnlocked, allLevelsUnlocked;
    private Ball ball;
    private int jumpCount, currentMaxAllowedJump;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Game Manager is null.");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        SaveSystem.Init();
    }

    public int JumpCount { get => jumpCount; set => jumpCount = value; }
    public bool Paused { get => paused; set => paused = value; }
    public int SavedLevel { get => savedLevel; set => savedLevel = value; }
    public bool AllBallsUnlocked { get => allBallsUnlocked; set => allBallsUnlocked = value; }
    public bool AllLevelsUnlocked { get => allLevelsUnlocked; set => allLevelsUnlocked = value; }
    public bool[] UnlockedBalls { get => unlockedBalls; set => unlockedBalls = value; }
    public bool[] UnlockedLevels { get => unlockedLevels; set => unlockedLevels = value; }
    public float Volume { get => volume; set => volume = value; }
    public int CurrentMaxAllowedJump { get => currentMaxAllowedJump; set => currentMaxAllowedJump = value; }

    private void Start()
    {
        Load();
    }

    public void UpdateJumpCount()
    {
        UIManager.Instance.UpdateJumpLeftCount(jumpCount);

        if(jumpCount <= 0)
        {
            ball = GameObject.Find("Ball").GetComponent<Ball>();
            ball.Die();
        }
    }

    public void ResetJumpCount()
    {
        jumpCount = CurrentMaxAllowedJump;
    }

    public int getDifficulty()
    {
        return difLevel;
    }

    public void setDifficulty(int level)
    {
        difLevel = level;
    }

    public void Save()
    {
        SaveObject save = new SaveObject(savedLevel, volume, difLevel, UnlockedBalls, UnlockedLevels, AllBallsUnlocked, AllLevelsUnlocked);

        string json = JsonUtility.ToJson(save);

        SaveSystem.Save(json);
    }

    private void Load()
    {
        string saveString = SaveSystem.Load();

        if (saveString != null)
        {
            SaveObject save = JsonUtility.FromJson<SaveObject>(saveString);

            savedLevel = save.lastLevel;
            setDifficulty(save.difLevel);
            volume = save.volume;
            for(int i = 0; i < unlockedBalls.Length; i++)
            {
                unlockedBalls[i] = save.unlockedBalls[i];
            }

            for (int i = 0; i < unlockedLevels.Length; i++)
            {
                unlockedLevels[i] = save.unlockedBalls[i];
            }

            allBallsUnlocked = save.allBallsUnlocked;
            allLevelsUnlocked = save.allLevelsUnlocked;

            Debug.Log("Loaded!");
        }
        else
        {
            Debug.Log("There is no save file!");
            setDifficulty(-1);
        }
    }

    private class SaveObject
    {
        public int lastLevel, difLevel;
        public float volume;
        public bool[] unlockedBalls, unlockedLevels;
        public bool allBallsUnlocked, allLevelsUnlocked;

        public SaveObject(int lastLevelIn, float volumeIn, int difLevelIn, bool[] unlockedBallsIn, bool[] unlockedLevelsIn, bool allBallsUnlockedIn, bool allLevelsUnlockedIn)
        {
            lastLevel = lastLevelIn;
            difLevel = difLevelIn;
            volume = volumeIn;
            unlockedBalls = unlockedBallsIn;
            unlockedLevels = unlockedLevelsIn;
            allBallsUnlocked = allBallsUnlockedIn;
            allLevelsUnlocked = allLevelsUnlockedIn;
        }
    }
}
