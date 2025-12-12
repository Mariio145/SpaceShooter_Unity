using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] GameObject boostPrefab;
    [SerializeField] protected int points = 100;
    [SerializeField] protected float speed = 2f;
    protected Vector3 linearVelocity = Vector3.left;

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(linearVelocity * (speed * Time.deltaTime));
        if (transform.position.x < -1.5f)
        {
            linearVelocity = Vector3.right;
        }
        
        if (transform.position.x > 2.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerShot"))
        {
            other.GetComponent<PlayerShot>().DestroyBullet();
            DestroyShip();
        }
    }

    public void DestroyShip()
    {
        SoundManager.Instance.PlaySfx("EnemyDie");
        if (Random.Range(0, 100) < 10)
            Instantiate(boostPrefab, transform.position, Quaternion.identity);
        
        PlayerSpaceShips.points += points;
        Destroy(gameObject);
    }
}
