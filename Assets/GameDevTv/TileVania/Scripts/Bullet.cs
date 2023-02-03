using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileVania;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidbody2D;

    [SerializeField] float bulletSpeed = 20f;

    GameObject player;
    float xSpeed = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(player.transform.localScale.x);
        xSpeed = player.transform.localScale.x;
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.velocity = new Vector2(bulletSpeed * xSpeed, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
