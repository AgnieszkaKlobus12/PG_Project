using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject main;
    public GameObject help;
    public GameObject @continue;
    public GameObject newGame;

    public TextMeshProUGUI slot1Mode;
    public TextMeshProUGUI slot1Level;
    public TextMeshProUGUI slot2Mode;
    public TextMeshProUGUI slot2Level;
    public TextMeshProUGUI slot3Mode;
    public TextMeshProUGUI slot3Level;

    public TextMeshProUGUI save1Mode;
    public TextMeshProUGUI save1Level;
    public TextMeshProUGUI save2Mode;
    public TextMeshProUGUI save2Level;
    public TextMeshProUGUI save3Mode;
    public TextMeshProUGUI save3Level;

    private Settings _settings;

    private void Start()
    {
        _settings = new Settings();
    }

    public void OnContinue()
    {
        @continue.SetActive(true);
        main.SetActive(false);
        var mode1 = _settings.GetMode(1);
        var mode2 = _settings.GetMode(2);
        var mode3 = _settings.GetMode(3);
        var level1 = _settings.GetLevel(1);
        var level2 = _settings.GetLevel(2);
        var level3 = _settings.GetLevel(3);
        if (mode1 != "empty")
        {
            slot1Mode.text = "Mode: " + mode1;
            slot1Level.text = "Level: " + level1;
        }
        else
        {
            slot1Mode.text = "empty";
            slot1Level.text = "empty";
        }

        if (mode2 != "empty")
        {
            slot2Mode.text = "Mode: " + mode2;
            slot2Level.text = "Level: " + level2;
        }
        else
        {
            slot2Mode.text = "empty";
            slot2Level.text = "empty";
        }

        if (mode3 != "empty")
        {
            slot3Mode.text = "Mode: " + mode3;
            slot3Level.text = "Level: " + level3;
        }
        else
        {
            slot3Mode.text = "empty";
            slot3Level.text = "empty";
        }
    }

    public void OnSlot(int slot)
    {
        PlayerPrefs.SetInt("Slot", slot);
        SceneManager.LoadScene(_settings.GetLevel(slot), LoadSceneMode.Single);
    }

    public void OnNewGame()
    {
        newGame.SetActive(true);
        main.SetActive(false);
        var mode1 = _settings.GetMode(1);
        var mode2 = _settings.GetMode(2);
        var mode3 = _settings.GetMode(3);
        var level1 = _settings.GetLevel(1);
        var level2 = _settings.GetLevel(2);
        var level3 = _settings.GetLevel(3);
        if (mode1 != "empty")
        {
            save1Mode.text = "Mode: " + mode1;
            save1Level.text = "Level: " + level1;
        }
        else
        {
            save1Mode.text = "empty";
            save1Level.text = "empty";
        }

        if (mode2 != "empty")
        {
            save2Mode.text = "Mode: " + mode2;
            save2Level.text = "Level: " + level2;
        }
        else
        {
            save2Mode.text = "empty";
            save2Level.text = "empty";
        }

        if (mode3 != "empty")
        {
            save3Mode.text = "Mode: " + mode3;
            save3Level.text = "Level: " + level3;
        }
        else
        {
            save3Mode.text = "empty";
            save3Level.text = "empty";
        }
    }

    public void OnHelp()
    {
        help.SetActive(true);
        main.SetActive(false);
    }

    public void OnBack()
    {
        main.SetActive(true);
        help.SetActive(false);
    }

    public void ContinueBack()
    {
        main.SetActive(true);
        @continue.SetActive(false);
    }

    public void NewGameBack()
    {
        main.SetActive(true);
        newGame.SetActive(false);
    }


    public void PickSlot(int slot)
    {
        PlayerPrefs.SetInt("Slot", slot);
    }

    public void StartNewMode(int mode)
    {
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }
}