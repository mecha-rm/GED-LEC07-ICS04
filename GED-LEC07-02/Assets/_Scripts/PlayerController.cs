using System.Collections;
using UnityEngine;
using Util;

public class PlayerController : MonoBehaviour
{
    public Speed verticalSpeed;
    public float maxSpeed;
    public Boundary boundary;
    public GameController gameController;
    public Transform bulletSpawn;
    public GameObject bullet;

    // private instance variables
    private AudioSource _thunderSound;
    private AudioSource _yaySound;
    private AudioSource _bulletSound;
    private Rigidbody2D _rigidbody2D;

    private bool isFiring = false;

    // TODO: create a reference to the BulletPoolManager here
    BulletPoolManager bulletPoolManager = null; // = new BulletPoolManager();

    // the maximum amount of bullets in the pool at a given time.
    // this is used to set the variable of the same name in 'BulletPoolManager'.
    public int MaxBullets = 3;

    // Start is called before the first frame update
    void Start()
    {
        _thunderSound = gameController.audioSources[(int)SoundClip.THUNDER];
        _yaySound = gameController.audioSources[(int)SoundClip.YAY];
        _bulletSound = GetComponent<AudioSource>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // Shoots bullet on a delay if button is pressed
        StartCoroutine(FireBullet());

        // bulletPoolManager = FindObjectOfType<BulletPoolManager>(); // finds the object type
        bulletPoolManager = BulletPoolManager.GetInstance(); // gets instance

        // destroys all bullets in the list.
        // this needs to be called because when the bullets are deleted, null values still remain in the list.
        bulletPoolManager.DestroyAllBullets();
    }

    // Update is called once per frame
    void Update()
    {
        // Move player
        Move();

        // Checks if shoot button is pressed
        ActionCheck();

        // Destroys bullet when it's off screen
        CheckBounds();

        // override max bullets value in case it changes.
        if (bulletPoolManager.MaxBullets < MaxBullets)
            bulletPoolManager.MaxBullets = MaxBullets;
        else // the maximum amount of bullets cannot be lowered.
            MaxBullets = bulletPoolManager.MaxBullets;
    }

    public void Move()
    {
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            _rigidbody2D.AddForce(new Vector2(verticalSpeed.max * Time.deltaTime, 0.0f));
        }

        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            _rigidbody2D.AddForce(new Vector2(verticalSpeed.min * Time.deltaTime, 0.0f));
        }

        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, 5.0f);
        _rigidbody2D.velocity *= 0.95f;
    }

    private void CheckBounds()
    {
        // check right boundary
        if (transform.position.x > boundary.Right)
        {
            transform.position = new Vector2(boundary.Right, transform.position.y);
        }

        // check left boundary
        if (transform.position.x < boundary.Left)
        {
            transform.position = new Vector2(boundary.Left, transform.position.y);
        }
    }

    private void ActionCheck()
    {
        // see Edit -> Project Settings -> Input
        if (Input.GetAxis("Jump") > 0)
        {
            isFiring = true;
        }
        else
        {
            isFiring = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Cloud":
                _thunderSound.Play();
                gameController.Lives -= 1;
                break;
            case "Island":
                _yaySound.Play();
                gameController.Score += 100;
                break;
        }
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            // Check every 0.15 seconds if shoot button is pressed
            yield return new WaitForSeconds(0.15f);
            if (isFiring)
            {
                _bulletSound.Play();

                //TODO: this code needs to change to user the BulletPoolManager's
                //TODO: GetBullet function which will return a reference to a 
                //TODO: bullet object. 
                //TODO: Ensure you position the new bullet at the bulletSpawn position

                // Instantiate(bullet, bulletSpawn.position, Quaternion.identity);

                // new code for getting a bullet from the pool.
                GameObject newBullet = bulletPoolManager.GetBullet();
                newBullet.transform.position = bulletSpawn.position;
                newBullet.transform.rotation = Quaternion.identity;
            }

        }
    }

}
