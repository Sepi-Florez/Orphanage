using System;
using System.Collections;
using UnityEngine;

public class CursorLock
{
    bool harry = true;
    public void Awake()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    /*public IEnumerator Lock (PlayerControl playerController, CombatManager weapon, float yieldTime)
    {
        //print("Meep");
        Debug.Log("A = " + Time.time);
        //yield(yieldTime);
        //Debug.Log("B = " + Time.time);
        if (Time.timeScale == 1.0)
        {
            yield return new WaitForSeconds(yieldTime);
            Time.timeScale = 0f;
            playerController.enabled = false; //Camera.main.GetComponentInParent<PlayerControl>().enabled = false;
            weapon.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1.0f;
            playerController.enabled = true; //Camera.main.GetComponentInParent<PlayerControl>().enabled = true;
            weapon.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
	}*/

    public void Lock (PlayerControl playerController, CombatManager weapon)
    {

        if (harry)
        {
           Time.timeScale = 0f;
            if (playerController != null)
            {
                playerController.enabled = false; //disables the playerController so you can't look around or move when timescale is set to 0
            }

            if (weapon != null) //disables the weaponScript because you might still be able to fire bullets while timescale is set to 0 so when you set it back to 1 again you shoot or swing X amount at the same time.
            {
                weapon.enabled = false;
            }
            harry = !harry;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
           Time.timeScale = 1.0f;
            if (playerController != null)
            {
                playerController.enabled = true;
            }
            if (weapon != null)
            {
                weapon.enabled = true;
            }
            Cursor.lockState = CursorLockMode.Locked;
            harry = !harry;
        }
    }
}