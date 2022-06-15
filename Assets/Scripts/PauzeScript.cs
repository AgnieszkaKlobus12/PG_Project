using UnityEngine;
using UnityEngine.SceneManagement;

public class PauzeScript : MonoBehaviour
{
    public void OnGOBack()
    {
        Time.timeScale = 1;
        var actions = GameObject.Find("Orc").GetComponent<PlayerController>();
        actions.PlayerActions.UI.Disable();
        if (new Settings().GetMode(PlayerPrefs.GetInt("Slot")) == "multiplayer")
        {
            actions.PlayerActions.Multiplayer.Enable();
        }
        else
        {
            actions.PlayerActions.Singleplayer.Enable();
        }
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}