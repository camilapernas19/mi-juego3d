
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    private AudioSource loseSound;

    private int count;
    private float movementX;
    private float movementY;
    private bool gameEnded = false;

    public float speed = 10;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // Efectos especiales
    public GameObject pickupEffect;
    public GameObject loseEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        GameObject loseSoundObject = GameObject.Find("LoseSound");

        if (loseSoundObject != null)
        {
            loseSound = loseSoundObject.GetComponent<AudioSource>();
        }

        count = 0;
        countText.text = "Count: 0";
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        if (gameEnded) return;

        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        if (gameEnded) return;

        Vector3 movement = new Vector3(movementX, 0f, movementY);

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameEnded) return;

        if (other.gameObject.CompareTag("PickUp"))
        {
            // Efecto al recoger cubitos
            if (pickupEffect != null)
            {
                GameObject effect = Instantiate(
                    pickupEffect,
                    other.transform.position,
                    Quaternion.identity
                );

                ParticleSystem[] particles =
                    effect.GetComponentsInChildren<ParticleSystem>(true);

                foreach (ParticleSystem particle in particles)
                {
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }

                Destroy(effect, 5f);
            }

            other.gameObject.SetActive(false);

            count++;

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }

            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 8)
        {
            gameEnded = true;

            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!";

            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

            if (enemy != null)
            {
                Destroy(enemy.transform.root.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameEnded) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameEnded = true;

            // Explosión al perder
            if (loseEffect != null)
            {
                GameObject explosion = Instantiate(
                    loseEffect,
                    transform.position,
                    Quaternion.identity
                );

                // Tamaño de la explosión
                explosion.transform.localScale = Vector3.one * 0.5f;

                // Activar todas las partículas
                ParticleSystem[] particles =
                    explosion.GetComponentsInChildren<ParticleSystem>(true);

                foreach (ParticleSystem particle in particles)
                {
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }

                Destroy(explosion, 5f);
            }

            if (loseSound != null)
            {
                loseSound.Play();
            }

            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";

            Destroy(gameObject);
        }
    }
}
