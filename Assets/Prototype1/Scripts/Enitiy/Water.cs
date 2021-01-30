using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Child"))
        {
            ChildMovement child = collision.gameObject.GetComponent<ChildMovement>();
            StartCoroutine(KillChild(child));
        }
    }

    IEnumerator KillChild(ChildMovement child)
    {
        yield return new WaitForSeconds(0.25f);
        child.SetDealth();
    }

}
