using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // Start is called before the first frame update
    private Coroutine coro;
    public float waitTime = 1.0f;

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the child or the uncle come into trigger collider wait for attack
        if (collision.gameObject.CompareTag("Uncle"))
        {
            UncleMovement uncle = collision.gameObject.GetComponent<UncleMovement>();
            coro = StartCoroutine(KillPlayer(uncle));
        }
        else if (collision.gameObject.CompareTag("Child"))
        {
            ChildMovement child = collision.gameObject.GetComponent <ChildMovement>();
            coro = StartCoroutine(KillPlayer(child));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the uncle press enter, this snake get killed.
        if (collision.gameObject.CompareTag("Uncle") && Input.GetKeyDown(KeyCode.Return))
        {
            //SendMessageUpwards("ForceSetTrigger", "Attack", SendMessageOptions.DontRequireReceiver);
            Debug.LogWarning(collision.gameObject, collision.gameObject);
            collision.gameObject.GetComponent<UncleMovement>().ForceSetTrigger("Attack");
            Debug.Log("Snake is killed");
            StopCoroutine(coro);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If they are no longer in attacking area stop;
        if (collision.gameObject.CompareTag("Uncle") || collision.gameObject.CompareTag("Child"))
        {
            StopCoroutine(coro);
        }
    }

    IEnumerator KillPlayer(UncleMovement uncle)
    {
        anim.SetTrigger("Prepare");
        yield return new WaitForSeconds(waitTime);
        anim.SetTrigger("Attack");
        uncle.SetDealth();
    }

    IEnumerator KillPlayer(ChildMovement child)
    {
        anim.SetTrigger("Prepare");
        yield return new WaitForSeconds(waitTime);
        anim.SetTrigger("Attack");
        child.SetDealth();
    }

}

