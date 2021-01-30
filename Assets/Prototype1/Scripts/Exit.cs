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

    private void Start()
    {
        playerCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerCount < 2) playerCount++;
        }

        if (playerCount == condition)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerCount > 0) playerCount--;
        }
    }

    IEnumerator LoadLevel(int index)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }
        
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(index);
    }
}