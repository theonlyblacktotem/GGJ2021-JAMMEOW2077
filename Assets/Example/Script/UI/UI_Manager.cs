using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public List<GameObject> _UI_list = new List<GameObject>();

    [Header("Fade black")]
    public GameObject fadeBlackOut;

    void Start()
    {
        _getAllChild();
    }

    void _getAllChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _UI_list.Add(transform.GetChild(i).gameObject);
        }
    }

    public void _openSelectedPage(GameObject nextUI)
    {
        nextUI.SetActive(true);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (_UI_list[i] != nextUI)
            {
                _UI_list[i].SetActive(false);
            }
        }

    }

    public void _exitGame(float exitDelayTime)
    {
        StartCoroutine("_waitExitGame", exitDelayTime);
    }

    IEnumerator _waitExitGame(float delayTime)
    {
        fadeBlackOut.SetActive(true);
        Debug.Log("Waiting to exit game...");

        yield return new WaitForSeconds(delayTime);

        Application.Quit();
        Debug.Log("Exit game.");
    }

    public void _changeToSelectedScene(string selectedSceneName)
    {
        StartCoroutine("_waitChangeToSelectedScene", selectedSceneName);
    }

    IEnumerator _waitChangeToSelectedScene(string selectedSceneName)
    {
        fadeBlackOut.SetActive(true);
        Debug.Log("Waiting to change scene...");

        yield return new WaitForSeconds(1.75f);

        SceneManager.LoadScene(selectedSceneName);
    }
}
