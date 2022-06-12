using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GemAdvanced : GemScript
{
    public int[] correctOrder;
    public GameObject[] objects;

    public override void Completed(int ind)
    {
        if (ind < correct.Length)
        {
            correct[ind] = true;
            order.Add(ind);
        }
    }

    public override void Failed(int ind)
    {
        if (ind > 0)
        {
            correct[ind] = false;
            order.Remove(ind);
        }
    }

    protected override void Update()
    {
        if (correct[correct.Length - 1] && !_showing)
        {
            _spriteRenderer.enabled = true;
            _showing = true;
            StartCoroutine(Show());
        }
        if (correctOrder.Length == order.Count)
        {
            if (correct[correct.Length - 1] && !_showing && CorrectOrder())
            {
                _spriteRenderer.enabled = true;
                _showing = true;
                StartCoroutine(Show());
            }
            else
            {
                foreach(GameObject obj in objects)
                {
                    obj.GetComponent<Animator>().enabled = false;
                }
            }
        }
    }

    private bool CorrectOrder()
    {
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i] != correctOrder[i])
            {
                return false;
            }
        }

        return true;
    }
}