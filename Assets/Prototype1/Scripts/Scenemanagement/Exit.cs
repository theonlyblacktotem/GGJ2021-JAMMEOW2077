using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    [Range(0, 2)] public int playerCount;
    [SerializeField] private int condition;
    public string newSceneName;

    private void Start()
    {
        playerCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Change Scene");
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerCount < 2) playerCount++;
        }

        if (playerCount == condition)
        {
            SceneManager.LoadScene(newSceneName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerCount > 0) playerCount--;
        }
    }
}
