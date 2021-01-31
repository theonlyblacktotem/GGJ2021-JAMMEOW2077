using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    [Range(0, 2)] public int playerCount;
    [SerializeField] private int condition;
    [SerializeField] private float waitTime;

    public Animator transition;

    public GameObject holyLight;

    public UncleMovement uncleM;
    public ChildMovement childM;

    private void Start()
    {
        playerCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Uncle"))
        {
            //if (playerCount < 2) playerCount++;
            uncleM = collision.gameObject.GetComponent<UncleMovement>();
        }

        if (collision.gameObject.CompareTag("Child"))
        {
            childM= collision.gameObject.GetComponent<ChildMovement>();

        }
        Debug.Log("condition " + playerCount);
        //if (playerCount == condition)
        //{
        //    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        //}

        if (childM && uncleM)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Uncle"))
        {
            //if (playerCount > 0) playerCount--;
            uncleM = null;
        }
        if (collision.gameObject.CompareTag("Child"))
        {
            childM = null;
        }
    }

    IEnumerator LoadLevel(int index)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        Instantiate(holyLight);
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(index);
    }
}