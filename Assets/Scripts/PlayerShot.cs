using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
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

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
