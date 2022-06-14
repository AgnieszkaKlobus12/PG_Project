using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GemScript : MonoBehaviour
{
    public GameObject orc, human;
    public GameObject newSkill;
    public float timeToHideView;
    public bool[] correct;
    public GameObject win;
    public GameObject lives;
    private int _nextIndex;
    private PlayerActions _playerActions;
    protected SpriteRenderer _spriteRenderer;
    private TextMeshProUGUI _text;
    private Vector3 _finalPosition;
    protected bool _showing;
    protected List<int> order;

    void Awake()
    {
        _playerActions = new PlayerActions();
        order = new List<int>();
        _nextIndex = 0;
        _showing = false;
        _finalPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _text = newSkill.GetComponentInChildren<Image>().GetComponentInChildren<TextMeshProUGUI>();
        transform.position += Vector3.up * 2;
    }

    protected virtual void Update()
    {
        if (correct.Length == 0 || AllCorrects() && !_showing)
        {
            _spriteRenderer.enabled = true;
            _showing = true;
            StartCoroutine(Show());
        }
    }

    private bool AllCorrects()
    {
        foreach (var c in correct)
        {
            if (!c) return false;
        }

        return true;
    }

    protected IEnumerator Show()
    {
        while (transform.position != _finalPosition)
        {
            transform.position -= Vector3.up * 0.1f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Show());
        }
    }

    public virtual void Completed(int ind = -1)
    {
        if (ind == -1)
        {
            ind = _nextIndex++;
        }
        if (ind < correct.Length)
        {
            correct[ind] = true;
        }
    }

    public virtual void Failed(int ind = -1)
    {
        if (ind == -1)
        {
            ind = _nextIndex--;
        }

        if (ind < correct.Length)
        {
            correct[ind] = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_showing || (!other.CompareTag("Player") && !other.CompareTag("Human"))) return;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 5":
                lives.SetActive(false);
                //win.SetActive(true);
                _playerActions.Singleplayer.Disable();
                _playerActions.Multiplayer.Disable();
                _playerActions.UI.Enable();
                break;
            case "Level 4":
                orc.GetComponent<PlayerController>().UnlockChargedAttack();
                human.GetComponent<PlayerController>().UnlockChargedAttack();
                _text.SetText("Klofalaekjarkjaftur - press S to attack with charge.\nJohn - press PgDown to attack with charge.");
                StartCoroutine(ShowNewSkillView());
                break;
            case "Level 3":
                orc.GetComponent<PlayerController>().UnlockDoubleJump();
                human.GetComponent<PlayerController>().UnlockDoubleJump();
                _text.SetText("Both of you can now double jump.");
                StartCoroutine(ShowNewSkillView());
                break;
            case "Level 2":
                orc.GetComponent<PlayerController>().UnlockAttack();
                human.GetComponent<PlayerController>().UnlockAttack();
                _text.SetText("Klofalaekjarkjaftur - press SPACE to attack.\nJohn - press ENTER to attack.");
                StartCoroutine(ShowNewSkillView());
                break;
            case "Level 1":
                orc.GetComponent<PlayerController>().UnlockJump();
                human.GetComponent<PlayerController>().UnlockJump();
                _text.SetText("Klofalaekjarkjaftur - press W to jump.\nJohn - press PgUp to jump.");
                StartCoroutine(ShowNewSkillView());
                break;
        }
        _showing = true;
        _spriteRenderer.enabled = false;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    IEnumerator ShowNewSkillView()
    {
        newSkill.SetActive(true);
        yield return new WaitForSeconds(timeToHideView);
        newSkill.SetActive(false);
        gameObject.SetActive(false);
    }
    
}