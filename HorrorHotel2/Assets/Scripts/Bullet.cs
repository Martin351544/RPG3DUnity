using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit" + collision.gameObject.name + "!");
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("hit");
            Destroy(gameObject);
        }
    }
}
