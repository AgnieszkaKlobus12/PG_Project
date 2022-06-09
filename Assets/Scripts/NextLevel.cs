using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextSceneName;
    public GameObject orc;
    public GameObject human;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Human"))
        {
            switch (nextSceneName)
            {
                case "Level 2":
                    if (orc.GetComponent<SetAnimatorParameter>().IsJumpEnabled() &&
                        human.GetComponent<SetAnimatorParameter>().IsJumpEnabled())
                    {
                        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
                    }
                    break;
            }
        }
    }
}