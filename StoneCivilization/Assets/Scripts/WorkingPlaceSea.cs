using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingPlaceSea : MonoBehaviour
{
    public bool isFree = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            isFree = false;
            var NPC = collision.gameObject;
            var info = NPC.GetComponent<CharacterInfo>();
            info.atWorkingPlaceSea = true;
            info.Animator.SetFloat("X", 0);
            info.Animator.SetFloat("Y", 0);
            info.Agent.velocity = Vector3.zero;
            info.Agent.isStopped = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            isFree = true;
            var NPC = collision.gameObject;
            NPC.GetComponent<CharacterInfo>().atWorkingPlaceSea = false;
        }
    }
}
