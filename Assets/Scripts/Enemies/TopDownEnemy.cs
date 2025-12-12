using UnityEngine;

public class TopDownEnemy : ShooterEnemy
{
    bool stop = true;
    protected override void Update()
    {
        transform.Translate(linearVelocity * (speed * Time.deltaTime));
        
        if (Mathf.Abs(transform.position.y) > 1)
        {
            linearVelocity *= -1;
        }
        
        if (transform.position.x < 1.7 && stop)
        {
            linearVelocity = Vector3.down;
            stop = false;
        }
    }
}
