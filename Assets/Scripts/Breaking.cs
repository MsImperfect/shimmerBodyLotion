//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//public class Breaking : MonoBehaviour
//{
//    [Header("Assign the broken prefab")]
//    public GameObject brokenPrefab;

//    public void Break()
//    {
//        if (brokenPrefab != null)
//        {
//            Debug.Log("The object has been touched");
//            Instantiate(brokenPrefab, transform.position, transform.rotation);
//            Destroy(gameObject);
//        }
//    }
//}
using UnityEngine;
using UnityEngine.InputSystem;   // <-- new API

public class Breaking : MonoBehaviour
{
    public GameObject brokenPrefab;
    public bool hasBroken = false;
    //public float destroyOriginalDelay = 0.05f;

    void Update()
    {
        
        hasBroken = true;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                var breaking = hit.collider.GetComponent<Breaking>();
                if (breaking != null)
                {
                    breaking.Break();
                }
            }
        }
        if (hasBroken) return;
    }

    public void Break()
    {
        if (brokenPrefab)
        {
            Instantiate(brokenPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}


