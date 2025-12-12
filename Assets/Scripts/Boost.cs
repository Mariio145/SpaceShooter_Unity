using System;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] float speed = 1.75f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] Vector3 direction;

    void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerSpaceShips>().ActivateBoost();
            Destroy(gameObject);
        }
    }
}
