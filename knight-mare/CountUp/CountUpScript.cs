using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountUpScript : MonoBehaviour
{
    public bool TimeOn = true;
    private float seconds = 00.0f;
    private float minutes = 00.0f;

    [SerializeField]
    Text TxtCountDown1, TxtCountDown2;

    public void Update()
    {
        seconds += Time.deltaTime;
        if (seconds > 60)
        {
            minutes += 1;
            seconds = 0;
        }
        TxtCountDown1.text = minutes + "m" + (int)seconds + "s";
        TxtCountDown2.text = minutes + "m" + (int)seconds + "s";
    }

    [SerializeField] Sprite slotWtihStars;
    [SerializeField] Image[] slots, slots2;
    private int x = 0;

    public void AddStarsInSlot()
    {
        slots[x].GetComponent<Image>().sprite = slotWtihStars;
        slots2[x].GetComponent<Image>().sprite = slotWtihStars;
        x++;
        x = Mathf.Clamp(x, 0, slots.Length);
    }
}
