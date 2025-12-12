using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSpaceShips : MonoBehaviour
{
    const float rawMoveThresholdForBraking = 0.5f;

    public static int points;
    
    [SerializeField] SpriteRenderer spriteRenderer;

    bool boostActive;
    
    [Header("Movement")]
    [SerializeField] float maxSpeed = 2f;
    [SerializeField] float acceleration = 300f;

    [Header("Shooting")] 
    [SerializeField] float shootCooldown = 0.1f;
    [SerializeField] Transform shootingPoint;
    [SerializeField] GameObject projectilePrefab;
    bool canShoot = true;
    
    [Header("Controls")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference shoot;
    [SerializeField] InputActionReference exit;

    [Header("Lives")] 
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] Image[] lives;
    public int numLives = 3;
    bool isInmune = false;
    
    [Header("GameOver")]
    [SerializeField] TextMeshProUGUI puntuation;
    [SerializeField] GameObject gameOverPanel;
    
    public static bool gameEnded;

    Vector2 rawMove;
    Vector2 currentVelocity = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        move.action.Enable();
        shoot.action.Enable();
        exit.action.Enable();

        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;
        shoot.action.started += OnShoot;
        exit.action.started += ExitGame;
        
        SoundManager.Instance.PlayMusic("Space Rider");
    }

    void FixedUpdate()
    {
        if (rawMove.magnitude < rawMoveThresholdForBraking)
        {
            currentVelocity *= 0.1f * Time.deltaTime;
        }

        currentVelocity += rawMove * (acceleration * Time.deltaTime);
        
        float linearVelocity = currentVelocity.magnitude;
        linearVelocity = Mathf.Clamp(linearVelocity, 0, maxSpeed);
        currentVelocity = currentVelocity.normalized * linearVelocity;
        
        transform.Translate(currentVelocity * Time.deltaTime);
        
        float xClamped = Mathf.Clamp(transform.position.x, -1.8f, 1.8f);
        float yClamped = Mathf.Clamp(transform.position.y, -0.9f, 0.9f);
        transform.position = new Vector3(xClamped, yClamped, 0);
        
        pointsText.text = "Puntos " + points;
    }

    void OnDisable()
    {
        move.action.Disable();
        shoot.action.Disable();
        exit.action.Disable();

        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
        shoot.action.started -= OnShoot;
        exit.action.started -= ExitGame;
    }

    void OnMove(InputAction.CallbackContext obj)
    {
        rawMove = obj.ReadValue<Vector2>();
    }

    void OnShoot(InputAction.CallbackContext obj)
    {
        if (canShoot)
        {
            SoundManager.Instance.PlaySfx("LaserPlayer");
            Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
            StartCoroutine(AttackCooldown());
        }
    }
    
    void ExitGame(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isInmune) return;

        if (other.CompareTag("EnemyShot"))
        {
            StartCoroutine(HitReceived());
            other.GetComponent<PlayerShot>().DestroyBullet();
        }
        else if (other.CompareTag("Enemy"))
        {
            StartCoroutine(HitReceived());
            other.GetComponent<BasicEnemy>().DestroyShip();
        }
    }

    void UpdateLives()
    {
        foreach (Image image in lives)
        {
            image.enabled = true;
        }

        if  (numLives == 2)
        {
            lives[2].enabled = false;
        }
        else if (numLives == 1)
        {
            lives[2].enabled = false;
            lives[1].enabled = false;
        }
        else
        {
            lives[2].enabled = false;
            lives[1].enabled = false;
            lives[0].enabled = false;
        }
    }

    void Die()
    {
        move.action.Disable();
        shoot.action.Disable();

        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
        shoot.action.started -= OnShoot;
        
        spriteRenderer.enabled = false;

        gameEnded = true;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        puntuation.text = points.ToString();
    }

    public void ActivateBoost()
    {
        if (boostActive) return;
        StartCoroutine(BoostPlayer());
    }

    IEnumerator HitReceived()
    {
        isInmune = true;
        numLives--;
        UpdateLives();
        SoundManager.Instance.PlaySfx("PlayerHit");
        if (numLives == 0)
        {
            Die();
            yield return new WaitForSeconds(1f);
            ShowGameOver();
            yield break;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.4f);
        }
        isInmune = false;
    }

    IEnumerator AttackCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    IEnumerator BoostPlayer()
    {
        boostActive = true;
        maxSpeed = 3f;
        shootCooldown = 0.01f;
        yield return new WaitForSeconds(5f);
        maxSpeed = 2f;
        shootCooldown = 0.25f;
        boostActive = false;
    }
}




