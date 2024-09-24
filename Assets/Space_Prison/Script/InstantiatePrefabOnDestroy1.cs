using UnityEngine;

public class InstantiatePrefabOnDestroy1 : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    public Transform parentTransform;

    // Flag to track whether the game is currently running
    private bool isGameRunning = false;

    // Method for instantiating the prefab
    private void InstantiatePrefab()
    {
        // Check if the game is currently running
        if (isGameRunning)
        {
            // Check if a parent is specified
            if (parentTransform != null)
            {
                // Instantiate the prefab as a child of the specified parent, at the parent's position
                Instantiate(prefabToInstantiate, parentTransform.position, parentTransform.rotation, parentTransform.parent);
                Debug.Log(000);
            }
            else
            {
                // Instantiate the prefab without a specific parent, at the current position
                Debug.Log(123);
                Instantiate(prefabToInstantiate, transform.position, transform.rotation);
            }
        }
    }

    // Called when the object is destroyed
    private void OnDestroy()
    {
        // Call the instantiation method when the object is destroyed
        InstantiatePrefab();
    }

    // Called when the game starts
    private void Start()
    {
        // Set the flag to indicate that the game is running
        isGameRunning = true;
    }

    // Called when the game is stopped
    private void OnApplicationQuit()
    {
        // Set the flag to indicate that the game is not running
        isGameRunning = false;
    }
}
