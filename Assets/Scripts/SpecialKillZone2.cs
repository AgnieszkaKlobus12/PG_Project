using UnityEngine;

public class SpecialKillZone2 : MonoBehaviour
{
    public GameObject specialLifeZone;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (!specialLifeZone.GetComponent<SpecialLifeZone>().IsPlayerDown())
            {
                col.GetComponent<SetAnimatorParameter>().Die();
            }
        }
        else if (col.CompareTag("Human"))
        {
            if (!specialLifeZone.GetComponent<SpecialLifeZone>().IsHumanDown())
            {
                {
                    col.GetComponent<SetAnimatorParameter>().Die();
                }
            }
        }
    }
}