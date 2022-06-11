using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogStart : MonoBehaviour
{
    private bool _completed;
    private PlayerController _orc;
    private PlayerController _human;

    private int _idx;

    public GameObject dialogCamera;
    public string[] texts;
    public string[] speakers;

    private TextMeshPro _speaker;
    private TextMeshPro _text;
    private VerticalLayoutGroup _picks;

    private void Start()
    {
        _idx = 0;
        _completed = false;
        _orc = GameObject.Find("Orc").GetComponent<PlayerController>();
        _human = GameObject.Find("Human").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_completed)
        {
            _orc.MovementEnabled = false;
            _human.MovementEnabled = false;
            NextText();
            dialogCamera.SetActive(true);
        }
    }

    private void NextText()
    {
        
    }
}
