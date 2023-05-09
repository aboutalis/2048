using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMainMenu : MonoBehaviour
{
    public GameObject instr;
    public GameObject pausedGO;
    public GameObject mainGO;
    public GameObject stepsGO;
    public GameObject gameType;
    public GameObject nameGO;

    public void Update()
    {
        EscapeButton();
    }

    public void EscapeButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (instr.activeSelf==true)
            {
                mainGO.SetActive(true);
                stepsGO.SetActive(false);
                instr.SetActive(false);
                pausedGO.SetActive(true);
            }
            else if (gameType.activeSelf==true)
            {
                gameType.SetActive(false);
                pausedGO.SetActive(true);
            }else if (nameGO.activeSelf==true)
            {
                nameGO.SetActive(false);
                pausedGO.SetActive(true);
            }
        }
    }
}
