using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointScripte : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] GameObject CanvasSucessLevel;
    AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<KnightScript>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Win");
            GameObject.Find("AudioMusic").GetComponent<AudioSource>().enabled = false;
            GameObject.Find("CanvasUI").SetActive(false);
            audioS.Play();
            particle.SetActive(true);
            StartCoroutine(pause());
        }
        IEnumerator pause()
        {
            yield return new WaitForSeconds(audioS.clip.length);
            CanvasSucessLevel.SetActive(true);
        }
    }
}
