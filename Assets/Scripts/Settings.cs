using UnityEngine;

public static class Settings
{
    //Save 1
    private static int _humanLives1;
    private static int _orcLives1;
    private static string _level1;
    private static string _mode1;

    //Save 2
    private static int _humanLives2;
    private static int _orcLives2;
    private static string _level2;
    private static string _mode2;

    //Save 3
    private static int _humanLives3;
    private static int _orcLives3;
    private static string _level3;
    private static string _mode3;

    public static int GetHumanLives(int save)
    {
        return save switch
        {
            1 => _humanLives1,
            2 => _humanLives2,
            3 => _humanLives3,
            _ => -1
        };
    }
    
    public static int GetOrcLives(int save)
    {
        return save switch
        {
            1 => _orcLives1,
            2 => _orcLives2,
            3 => _orcLives3,
            _ => -1
        };
    }
    
    public static void SetHumanLives(int save, int lives)
    { 
        switch (save)
        {
            case 1:
                _humanLives1 = lives;
                break;
            case 2:
                _humanLives2 = lives;
                break;
            case 3:
                _humanLives3 = lives;
                break;
        };
    }
    
    public static void SetOrcLives(int save, int lives)
    { 
        switch (save)
        {
            case 1:
                _orcLives1 = lives;
                break;
            case 2:
                _orcLives2 = lives;
                break;
            case 3:
                _orcLives3 = lives;
                break;
        };
    }

    public static string GetLevel(int save)
    {
        return save switch
        {
            1 => _level1,
            2 => _level2,
            3 => _level3,
            _ => "empty"
        };
    }
    
    public static void SetLevel(int save, string level)
    { 
        switch (save)
        {
            case 1:
                _level1 = level;
                break;
            case 2:
                _level2 = level;
                break;
            case 3:
                _level3 = level;
                break;
        };
    }
    
    public static string GetMode(int save)
    {
        return save switch
        {
            1 => _mode1,
            2 => _mode2,
            3 => _mode3,
            _ => "empty"
        };
    }
    
    public static void SetMode(int save, string mode)
    { 
        switch (save)
        {
            case 1:
                _mode1 = mode;
                break;
            case 2:
                _mode2 = mode;
                break;
            case 3:
                _mode3 = mode;
                break;
        };
    }
}