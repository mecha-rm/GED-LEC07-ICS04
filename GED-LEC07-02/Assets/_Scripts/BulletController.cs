using UnityEngine;
using Util;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 0.1f;
    public Boundary boundary;

    // TODO: create a reference to the BulletPoolManager
    BulletPoolManager bulletPoolManager = null; // BulletPoolManager.GetInstance();

    void Start()
    {
        boundary.Top = 2.45f;
        // bulletPoolManager = FindObjectOfType<BulletPoolManager>(); // finds bullet pool manager object.
        bulletPoolManager = BulletPoolManager.GetInstance(); // gets instance
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBounds();
    }

    private void Move()
    {
        transform.position += new Vector3(0.0f, bulletSpeed * Time.deltaTime, 0.0f);
    }

    private void CheckBounds()
    {
        if (transform.position.y >= boundary.Top)
        {
            //TODO: This code needs to change to use the BulletPoolManager's
            //TODO: ResetBullet function which will return the bullet to the pool

            // Destroy(this.gameObject); // destroys bullet
            bulletPoolManager.ResetBullet(this.gameObject); // puts bullet back in queue.
        }
    }
}
