using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Main;
    public GameObject Help;
    public GameObject Continue;
    public GameObject NewGame;

    public TextMeshProUGUI Slot1Mode;
    public TextMeshProUGUI Slot1Level;
    public TextMeshProUGUI Slot2Mode;
    public TextMeshProUGUI Slot2Level;
    public TextMeshProUGUI Slot3Mode;
    public TextMeshProUGUI Slot3Level;
    
    public TextMeshProUGUI Save1Mode;
    public TextMeshProUGUI Save1Level;
    public TextMeshProUGUI Save2Mode;
    public TextMeshProUGUI Save2Level;
    public TextMeshProUGUI Save3Mode;
    public TextMeshProUGUI Save3Level;

    public void OnContinue()
    {
        Continue.SetActive(true);
        Main.SetActive(false);
        var mode1 = Settings.GetMode(1);
        var mode2 = Settings.GetMode(2);
        var mode3 = Settings.GetMode(3);
        var level1 = Settings.GetLevel(1);
        var level2 = Settings.GetLevel(2);
        var level3 = Settings.GetLevel(3);
        if (mode1 != "empty")
        {
            Slot1Mode.text = "Mode: " + mode1;
            Slot1Level.text = "Level: " + level1;
        }
        else
        {
            Slot1Mode.text = "empty";
            Slot1Level.text = "empty";
        }
        if (mode2 != "empty")
        {
            Slot2Mode.text = "Mode: " + mode2;
            Slot2Level.text = "Level: " + level2;
        }
        else
        {
            Slot2Mode.text = "empty";
            Slot2Level.text = "empty";
        }
        if (mode3 != "empty")
        {
            Slot3Mode.text = "Mode: " + mode3;
            Slot3Level.text = "Level: " + level3;
        }
        else
        {
            Slot3Mode.text = "empty";
            Slot3Level.text = "empty";
        }
    }

    public void OnSlot(int slot)
    {
        PlayerPrefs.SetInt("Slot", slot);
        SceneManager.LoadScene(Settings.GetLevel(slot), LoadSceneMode.Single);
    }

    public void OnNewGame()
    {
        NewGame.SetActive(true);
        Main.SetActive(false);
        var mode1 = Settings.GetMode(1);
        var mode2 = Settings.GetMode(2);
        var mode3 = Settings.GetMode(3);
        var level1 = Settings.GetLevel(1);
        var level2 = Settings.GetLevel(2);
        var level3 = Settings.GetLevel(3);
        if (mode1 != "empty")
        {
            Save1Mode.text = "Mode: " + mode1;
            Save1Level.text = "Level: " + level1;
        }
        else
        {
            Save1Mode.text = "empty";
            Save1Level.text = "empty";
        }
        if (mode2 != "empty")
        {
            Save2Mode.text = "Mode: " + mode2;
            Save2Level.text = "Level: " + level2;
        }
        else
        {
            Save2Mode.text = "empty";
            Save2Level.text = "empty";
        }
        if (mode3 != "empty")
        {
            Save3Mode.text = "Mode: " + mode3;
            Save3Level.text = "Level: " + level3;
        }
        else
        {
            Save3Mode.text = "empty";
            Save3Level.text = "empty";
        }
    }

    public void OnHelp()
    {
        Help.SetActive(true);
        Main.SetActive(false);
    }

    public void OnBack()
    {
        Main.SetActive(true);
        Help.SetActive(false); 
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