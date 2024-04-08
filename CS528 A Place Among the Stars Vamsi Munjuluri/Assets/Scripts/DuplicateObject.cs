using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateObject : MonoBehaviour
{
    public GameObject objectToDuplicate;
    void OnEnable()
    {
        Instantiate(objectToDuplicate, objectToDuplicate.transform.position, objectToDuplicate.transform.rotation);
    }
}
