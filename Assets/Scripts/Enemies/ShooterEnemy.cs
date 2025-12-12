using System.Collections;
using UnityEngine;

public class ShooterEnemy : BasicEnemy
{
    [SerializeField] float shootCooldown;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject bulletPrefab;
    void OnEnable()
    {
        StartCoroutine(Shoot());
    }
    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(linearVelocity * (speed * Time.deltaTime));
        
        if (transform.position.x < -2.5f)
        {
            Destroy(gameObject);
        }
    }

    protected IEnumerator Shoot()
    {
        while (true)
        {
            SoundManager.Instance.PlaySfx("LaserEnemy");
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(shootCooldown);
        }
    }
}
