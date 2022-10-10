using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float MenuTransitionDuration;

    public bool pausedGame;
    public bool beginGame;

    public CanvasGroup target;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        if (target != null)
            AudioManager.instance.target = target;
    }

    public void OpenOption() {
        AudioManager.instance.EnterOptions(target);
        AudioManager.instance.target = target;
    }

    public void CloseOptions() {
        AudioManager.instance.ExitOptions(target);
        AudioManager.instance.target = target;
    }

    public void PlaySoundEffect(AudioClip ac) {
        AudioManager.instance.PlaySound(ac);
    }

    public void FateMusic(AudioClip ac) {
        AudioManager.instance.FadeMusic(ac);
    }

    public void BeginGame() {
        beginGame = true;
    }

    public void EndGame() {
        beginGame = false;
    }

    public void PauseGame() {
        pausedGame = !pausedGame;
        beginGame = !pausedGame;

        if (AudioManager.instance != null)
            AudioManager.instance.MusicPauseGame(pausedGame);
        else Debug.Log("Audio Manager is Missing");

        if (pausedGame) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void ShowCanvasGroup(CanvasGroup CG) {
        if (CG != null) {
            StartCoroutine(ShowCanvasGroupAlpha(CG));
        } else {
            Debug.Log("Panel with Canvas Groups is Missing");
        }
    }

    IEnumerator ShowCanvasGroupAlpha(CanvasGroup CG) {
        CG.blocksRaycasts = true;
        float currentTime = 0f;
        while (currentTime < MenuTransitionDuration) {
            CG.alpha = Mathf.Lerp(0, 1, currentTime / MenuTransitionDuration);
            currentTime += Time.fixedDeltaTime;
            yield return null;
        }
        CG.alpha = 1;
        yield return null;
    }

    public void HideCanvasGroup(CanvasGroup CG) {
        if (CG != null) {
            StartCoroutine(HideCanvasGroupAplha(CG));
        } else {
            Debug.Log("Panel with Canvas Groups is Missing");
        }
    }

    IEnumerator HideCanvasGroupAplha(CanvasGroup CG) {
        CG.blocksRaycasts = false;
        float currentTime = 0f;
        while (currentTime < MenuTransitionDuration) {
            CG.alpha = Mathf.Lerp(1, 0, currentTime / MenuTransitionDuration);
            currentTime += Time.fixedDeltaTime;
            yield return null;
        }
        CG.alpha = 0;
        yield return null;
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    
    


}
