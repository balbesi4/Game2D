using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyControl : MonoBehaviour
{
    public GameObject WoodenWreck;
    public AudioClip CrackSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BulletControl bulletControl))
        {
            var objects = GetComponentsInParent<Transform>()[1];
            var objectPos = gameObject.transform.position;
            AudioSource.PlayClipAtPoint(CrackSound, Camera.main.transform.position);
            Destroy(collision.gameObject);
            Destroy(gameObject);

            Instantiate(WoodenWreck, objectPos, Quaternion.identity, objects.transform);
        }
    }

    public void DestroyItself()
    {
        var objects = GetComponentsInParent<Transform>()[1];
        var objectPos = gameObject.transform.position;
        Destroy(gameObject);

        Instantiate(WoodenWreck, objectPos, Quaternion.identity, objects.transform);
    }
}
