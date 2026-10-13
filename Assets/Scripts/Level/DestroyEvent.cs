using Unity.VisualScripting;
using UnityEngine;

public class DestroyEvent : MonoBehaviour
{

    public void Update()
    {
        Destroy(gameObject, 2);
    }
    
}
