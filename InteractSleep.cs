using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSleep : MonoBehaviour
{
    private PauseMenu pauseMenu;
    private GameController gameController;
    private TimeSkipUI tsUI;

    void Start()
    {
        tsUI = FindAnyObjectByType<TimeSkipUI>();
        gameController = FindObjectOfType<GameController>();
        pauseMenu = FindObjectOfType<PauseMenu>();

    }
    private void OpenUI()
    {
        pauseMenu.tipMenuText.text = "INTERACT (" + KeyBindingManager.GetKeyCode(KeyAction.interactKey) + ") KEY TO:";
        pauseMenu.pickupItemTipText.text = "Sleep";

        //open pickup (popup) screen
        pauseMenu.pickupItemScreen.gameObject.SetActive(true);
    }
    private void CloseUI()
    {
        gameController.DisableInvUIDelayFunc();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if player
        if (other.CompareTag("Player"))
            OpenUI();
    }
    private void OnTriggerStay(Collider other)
    {
        //if player
        if (other.CompareTag("Player"))
        {


            if (KeyBindingManager.GetKey(KeyAction.interactKey))
            {
                CloseUI();
                OpenWaitMenu();

            }


        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseUI();
        }
    }

    private void OpenWaitMenu()
    {
        PauseMenu.gameIsPaused = true;
        pauseMenu.PauseGame();
        pauseMenu.pauseMenuScreen.SetActive(true);
        pauseMenu.pauseMenuObj.SetActive(false);
        pauseMenu.timeMenuObj.SetActive(true);
        tsUI.backButton.interactable = false;
        tsUI.isSleeping = true;
    }


}
