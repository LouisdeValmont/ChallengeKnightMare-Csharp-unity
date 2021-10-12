using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScripte : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    [SerializeField] GameObject canvasA, canvasB;

    public void SelectLevel()
    {
        canvasA.SetActive(false);
        canvasB.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // mute Sound
    Image imMute;
    [SerializeField] Sprite spMute, spSound;
    bool isPlaying = true;

    void Start()
    {
        imMute = GameObject.Find("ImMute").GetComponent<Image>();
        bool mute = PlayerPrefs.GetInt("mute") == 0 ? false : true;
        imMute.sprite = mute ? spMute : spSound;
    }

    public void MuteSound()
    {
        isPlaying = !isPlaying;
        imMute.sprite = isPlaying ? spMute : spSound;
        PlayerPrefs.SetInt("mute", isPlaying ? 1 : 0);
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel") + 1);
    }
}
