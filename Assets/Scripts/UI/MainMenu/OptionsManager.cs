using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] GameObject[] options;
    [SerializeField] Sprite activeImage;
    [SerializeField] Sprite inactiveImage;
    private int currentOptionIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        options[currentOptionIndex].GetComponent<Image>().sprite = activeImage;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            currentOptionIndex++;
            currentOptionIndex = Mathf.Clamp(currentOptionIndex, 0, options.Length - 1);

            GameObject currentOption = options[currentOptionIndex];
            GameObject previousOption = options[currentOptionIndex - 1];

            currentOption.GetComponent<Image>().sprite = activeImage;
            previousOption.GetComponent<Image>().sprite = inactiveImage;

            currentOption.GetComponentInChildren<SingleOption>().ActiveSprite();
            previousOption.GetComponentInChildren<SingleOption>().InactiveSprite();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            currentOptionIndex--;
            currentOptionIndex = Mathf.Clamp(currentOptionIndex, 0, options.Length - 1);

            GameObject currentOption = options[currentOptionIndex];
            GameObject previousOption = options[currentOptionIndex + 1];

            currentOption.GetComponent<Image>().sprite = activeImage;
            previousOption.GetComponent<Image>().sprite = inactiveImage;

            currentOption.GetComponentInChildren<SingleOption>().ActiveSprite();
            previousOption.GetComponentInChildren<SingleOption>().InactiveSprite();
        }
    }
}
