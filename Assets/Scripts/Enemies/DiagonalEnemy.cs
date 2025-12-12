using UnityEngine;

public class DiagonalEnemy : ShooterEnemy
{
    void OnEnable()
    {
        linearVelocity = Vector3.left + Vector3.up;
        StartCoroutine(Shoot());
    }
    protected override void Update()
    {
        transform.Translate(linearVelocity * (speed * Time.deltaTime));
        
        if (transform.position.y > 0.9)
        {
            linearVelocity = Vector3.down + Vector3.left;
        }
        else if (transform.position.y < -0.9)
        {
            linearVelocity = Vector3.up + Vector3.left;
        }
    }
}
