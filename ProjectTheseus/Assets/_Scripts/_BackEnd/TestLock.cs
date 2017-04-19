using UnityEngine;

public class TestLock : MonoBehaviour
{
    CursorLock cursorLock = new CursorLock();
    void Awake()
    {
        cursorLock.Awake();
    }
    void Update ()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            cursorLock.Lock(Camera.main.GetComponentInParent<PlayerControlV2>(), Camera.main.GetComponentInChildren<CombatManager>());
        }
	}
}