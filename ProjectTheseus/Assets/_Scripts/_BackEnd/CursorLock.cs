using UnityEngine;

public class CursorLock
{
    public void Awake()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
	public void Lock (PlayerControlV2 playerController, CombatManager weapon)
    {
        if(Time.timeScale == 1.0)
        {
            Time.timeScale = 0f;
            playerController.enabled = false; //Camera.main.GetComponentInParent<PlayerControlV2>().enabled = false;
            weapon.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1.0f;
            playerController.enabled = true; //Camera.main.GetComponentInParent<PlayerControlV2>().enabled = true;
            weapon.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
	}
}