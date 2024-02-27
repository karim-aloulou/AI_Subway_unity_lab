using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 200f;
    public float jumpForce = 500f;
    public LayerMask groundLayer;

    public enum PlayerPosition { Left, Center, Right };
    public PlayerPosition currentPosition = PlayerPosition.Center;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, speed * Time.deltaTime);

        // Détection de l'input pour se déplacer latéralement
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        // Détection de l'input pour sauter
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Raycast vers le bas pour détecter les objets sur lesquels le joueur doit sauter
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
        {
            Debug.DrawRay(transform.position, Vector3.down * 1.1f, Color.red); // Debug du raycast
            // Si le joueur touche un objet au sol, il est sur une plateforme et peut sauter à nouveau
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                rb.useGravity = false;
                rb.velocity = new Vector3(0, 0, speed * Time.deltaTime);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * 1.1f, Color.green); // Debug du raycast
            // Si le joueur ne touche pas d'objet au sol, il tombe librement
            rb.useGravity = true;
        }
    }

    // Déplacement vers la droite
    void MoveRight()
    {
        if (currentPosition == PlayerPosition.Left)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            currentPosition = PlayerPosition.Center;
        }
        else if (currentPosition == PlayerPosition.Center)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            currentPosition = PlayerPosition.Right;
        }
    }

    // Déplacement vers la gauche
    void MoveLeft()
    {
        if (currentPosition == PlayerPosition.Right)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            currentPosition = PlayerPosition.Center;
        }
        else if (currentPosition == PlayerPosition.Center)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            currentPosition = PlayerPosition.Left;
        }
    }

    // Sauter
    void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer))
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }
}
