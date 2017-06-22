using UnityEngine;

public class TestLock : MonoBehaviour
{
    CursorLock cursorLock = new CursorLock();
    public PlayerControlPhysics playerController;
    public CombatManager combatManager;
    //public HealthManager healthManager;
    //HealthManager healthManager = new HealthManager();
    void Awake()
    {
        cursorLock.Awake();
    }
    void Update ()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            cursorLock.Lock(playerController, combatManager);
            //PlayerControlPhysics.Shake(0.1f, 0.5f);
        }
	}
}