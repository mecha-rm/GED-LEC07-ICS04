using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// TODO: Bonus - make this class a Singleton!

[System.Serializable] // needed for singleton bonus
public class BulletPoolManager
{
    // instance of bullet pool manager.
    private static BulletPoolManager instance = null;

    // bullet
    public GameObject bullet;

    // TODO: create a structure to contain a collection of bullets
    private Queue<GameObject> bulletPool;

    // maximum bullets
    public int MaxBullets = 3;

    // constructor - used for instancing (for bonus)
    private BulletPoolManager()
    {
        Start();
    }

    // gets instance of class (for bonus)
    public static BulletPoolManager GetInstance()
    {
        if (instance == null)
        {
            instance = new BulletPoolManager(); // creates instance
        }

        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: add a series of bullets to the Bullet Pool
        // finds bullet

        // creates the bullet.
        if (bullet == null)
            bullet = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullet.prefab", typeof(Object));

        // builds bullets
        _BuildBulletPool();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //TODO: modify this function to return a bullet from the Pool
    public GameObject GetBullet()
    {
        // creates a new bullet if there are none available.
        // alternatively, you could recall a bullet that's already been fired.
        if (bulletPool.Count == 0)
        {
            bulletPool.Enqueue(MonoBehaviour.Instantiate(bullet));
            MaxBullets++; // a new max has been set.
        }

        // gets the bullet to be fired.
        GameObject firedBullet = bulletPool.Dequeue();
        firedBullet.SetActive(true);

        // return bullet;
        return firedBullet; // returns the bullet that has been fired.
    }

    // TODO: modify this function to reset/return a bullet back to the Pool 
    public void ResetBullet(GameObject bullet)
    {
        bullet.transform.position = this.bullet.transform.position; // set as original bullet's position.
        bullet.SetActive(false); // set the bullet as being inactive
        bulletPool.Enqueue(bullet); // put bullet back in queue.
    }

    // Task 4: builds the bullet pool.
    private void _BuildBulletPool()
    {
        // if the bullet pool hasn't been made yet.
        if (bulletPool == null)
            bulletPool = new Queue<GameObject>();

        // if no bullet has been set.
        if (bullet == null)
            return;

        // the bullet shoudl not be visible.
        bullet.SetActive(false);

        // while there are still bullets to load in. 
        while (bulletPool.Count < MaxBullets)
        {
            bulletPool.Enqueue(MonoBehaviour.Instantiate(bullet));
        }
    }

    // Task 4: size of bullet pool
    public int GetCurrentBulletPoolSize()
    {
        return bulletPool.Count;
    }

    // Task 4: returns whether the bullet pool is empty or not.
    public bool IsBulletPoolEmpty()
    {
        return bulletPool.Count == 0;
    }

    // destroys all bullets in the queue.
    public void DestroyAllBullets()
    {
        // destroys all bullets.
        while (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            MonoBehaviour.Destroy(bullet);
        }

    }

    // sets the bullet used for firing. This removes all existing bullets, and creates new ones.
    public void SetBullet(GameObject bullet)
    {
        DestroyAllBullets();
        this.bullet = bullet;
        _BuildBulletPool(); // rebuilds the bullet pool.
    }
}
