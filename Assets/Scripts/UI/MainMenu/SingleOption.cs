using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleOption : MonoBehaviour
{
    [SerializeField] Sprite inactiveImage;
    [SerializeField] Sprite activeImage;
    
    public void ActiveSprite() {
        GetComponent<Image>().sprite = activeImage;
    }

    public void InactiveSprite() {
        GetComponent<Image>().sprite = inactiveImage;
    }
}
