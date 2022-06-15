using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextSceneName;
    public GameObject orc;
    public GameObject human;
    public Settings Settings;
    public GameObject gem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((!other.CompareTag("Player") && !other.CompareTag("Human")) || 
            !gem.gameObject.GetComponent<GemScript>().isShowing())
            return;

        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }
}