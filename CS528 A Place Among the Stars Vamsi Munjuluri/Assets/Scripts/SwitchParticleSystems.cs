using UnityEngine;

public class SwitchParticleSystems : MonoBehaviour
{
    public GameObject firstObject;
    public GameObject secondObject;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the first object is on and the second is off at the start.
        firstObject.SetActive(true);
        secondObject.SetActive(false);
    }

    // Method to be called by the button's OnClick event
    public void Toggle()
    {
        // Check the current active state and switch them
        firstObject.SetActive(!firstObject.activeSelf);
        secondObject.SetActive(!secondObject.activeSelf);
    }
}
