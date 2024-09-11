using UnityEngine;
using System.Collections;

public class DeleteAfterTime : MonoBehaviour
{
    // Time before the object is deleted
    public float timeBeforeDelete = 3f;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to delete the object after a set time
        StartCoroutine(DeleteAfterTimeCoroutine());
    }

    IEnumerator DeleteAfterTimeCoroutine()
    {
        // Wait for the specified time before deleting the object
        yield return new WaitForSeconds(timeBeforeDelete);

        // Delete the object
        Destroy(gameObject);
    }
}
