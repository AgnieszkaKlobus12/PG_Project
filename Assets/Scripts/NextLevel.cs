using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextSceneName;
    public GameObject orc;
    public GameObject human;
    public Settings Settings;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Human")) return;
        switch (nextSceneName)
        {
            case "Level 2":
                if (orc.GetComponent<PlayerController>().IsJumpEnabled() &&
                    human.GetComponent<PlayerController>().IsJumpEnabled())
                {
                    SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
                }

                break;
            case "Level 3":
                if (orc.GetComponent<PlayerController>().IsAttackEnabled() &&
                    human.GetComponent<PlayerController>().IsAttackEnabled())
                {
                    SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
                }

                break;
            case "Level 4":
                if (orc.GetComponent<PlayerController>().IsDoubleJumpEnabled() &&
                    human.GetComponent<PlayerController>().IsDoubleJumpEnabled() &&
                    GameObject.Find("PoisonousMushroom") == null)
                {
                    SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
                }

                break;
            case "Level 5":
                if (orc.GetComponent<PlayerController>().IsChargedAttackEnabled() &&
                    human.GetComponent<PlayerController>().IsChargedAttackEnabled())
                {
                    SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
                }

                break;
        }
    }
}