using UnityEngine;

public class CursorLock {
    static bool isLocked = true;

    static PlayerControlPhysics playerController;
    static CombatManager weapon;

    public void Awake() {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #region

    public static void SetPlayerScripts(PlayerControlPhysics _playerController, CombatManager _weapon) {
        playerController = _playerController;
        weapon = _weapon;
    }
    public static void SetPlayerScripts(PlayerControlPhysics _playerController) {
        playerController = _playerController;
    }
    public static void SetPlayerScripts(CombatManager _weapon) {
        weapon = _weapon;
    }

    #endregion

    public static void Lock() {
        Time.timeScale = isLocked ? 0f : 1.0f;
        if (playerController != null) {
            playerController.enabled = !isLocked; //disables the playerController so you can't look around or move when timescale is set to 0
        }

        if (weapon != null) //disables the weaponScript because you might still be able to fire bullets while timescale is set to 0 so when you set it back to 1 again you shoot or swing X amount at the same time.
        {
            weapon.enabled = !isLocked;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = isLocked;
        isLocked = !isLocked;
    }
}