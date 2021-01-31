using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] Text txt_dialogueText;
    [SerializeField] Animator animMain;
    [SerializeField] Animator animFade;
    [Space]
    [SerializeField] string sceneToLoad;
    [SerializeField] int currentSceneIndex = 0;
    public CutsceneDialogue[] cutsceneDialogues;

    Coroutine currentCouroutine;

    void Start()
    {
        if (!animMain) animMain = FindObjectOfType<Animator>();
        if (!animFade) Debug.LogError("XXXXXXXXXXXXXXX");
        if (!animFade.gameObject.activeSelf)
        {
            animFade.gameObject.SetActive(true);
        }
        foreach (var item in cutsceneDialogues)
        {
            item.sceneObject.SetActive(false);
        }

        currentCouroutine = StartCoroutine(PlayScene(0, cutsceneDialogues[0].sceneDuration));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            CheckFinishCutscene(true);
        }
    }

    private void CheckFinishCutscene(bool isSkip = false)
    {
        if (currentCouroutine != null) StopCoroutine(currentCouroutine);
        currentSceneIndex++;
        if (currentSceneIndex >= cutsceneDialogues.Length)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            animMain.SetTrigger("Next");
            currentCouroutine = StartCoroutine(PlayScene(currentSceneIndex, cutsceneDialogues[0].sceneDuration, isSkip));
        }
    }

    IEnumerator PlayScene(int index, float duration, bool isSkip = false)
    {
        if (isSkip && !cutsceneDialogues[index-1].noFadeIn)
        {
            animFade.SetTrigger("FadeIn");
            yield return new WaitForSeconds(0.5f);
        }
        if(!cutsceneDialogues[index].noFadeOut) animFade.SetTrigger("FadeOut");
        if (index > 0)
            cutsceneDialogues[index - 1].sceneObject.SetActive(false);
        cutsceneDialogues[index].sceneObject.SetActive(true);
        if (cutsceneDialogues[index].dialogue.Length > 1)
        {
            // Something about playing multiple dialogue
            StartCoroutine(ShowMultipleDialogue(index, cutsceneDialogues[index].dialogueDuration));
        }
        else
        {
            txt_dialogueText.text = cutsceneDialogues[index].dialogue[0];
        }
        yield return new WaitForSeconds(duration - 0.5f);
        if (!isSkip && !cutsceneDialogues[index].noFadeIn) animFade.SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.5f);

        CheckFinishCutscene();
    }

    IEnumerator ShowMultipleDialogue(int index, float dialogueDIr)
    {
        for (int i = 0; i < cutsceneDialogues[index].dialogue.Length; i++)
        {
            txt_dialogueText.text = cutsceneDialogues[index].dialogue[i];
            yield return new WaitForSeconds(dialogueDIr);
        }
    }

}

[System.Serializable]
public struct CutsceneDialogue
{
    public float sceneDuration;
    public string[] dialogue;
    public GameObject sceneObject;
    public float dialogueDuration;
    public bool noFadeIn;
    public bool noFadeOut;
}
