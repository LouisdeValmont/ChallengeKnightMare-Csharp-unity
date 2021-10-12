using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScripte : MonoBehaviour
{
    [SerializeField] string[] tags;

    void Start()
    {
        foreach (string tag in tags)
        {
            GameObject[] go = GameObject.FindGameObjectsWithTag(tag);

            foreach(GameObject g in go)
            {
                g.SetActive(false);
            }
        }

        GameObject.Find("AudioMusic").GetComponent<AudioSource>().enabled = false;
    }

    private void Awake()
    {
        if(PlayerPrefs.GetInt("mute") == 0)
        {
            GetComponent<AudioSource>().enabled = false;
        }
        
    }

}
