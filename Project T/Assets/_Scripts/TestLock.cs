using UnityEngine;

public class TestLock : MonoBehaviour
{
    CursorLock cursorLock = new CursorLock();
    public HealthManager healthManager;
    //HealthManager healthManager = new HealthManager();
    void Awake()
    {
        cursorLock.Awake();
    }
    void Update ()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            //cursorLock.Lock(Camera.main.GetComponentInParent<PlayerControl>(), Camera.main.GetComponentInChildren<CombatManager>(), 0.5f);
            healthManager.UpdateHP(-10);
        }
	}
}