using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    public static bool IsGamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(IsGamePaused);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) {
            IsGamePaused = !IsGamePaused;
            mainMenu.SetActive(IsGamePaused);
            if(IsGamePaused) {
                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1;
            }
        }
    }
}
