using FMODUnity;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float pointsPerSecond;

    public float maxHealth;
    public float maxSizeMult;
    public float minSizeMult;

    [SerializeField] private bool isImune = false;

    [HideInInspector] public float Health 
    { 
        get { return _health; }
        set 
        { 
            if (value < 0) { _health = 0; Kill(); }
            else if (value > maxHealth) { _health = maxHealth; }
            else { _health = value; }

            float mult = Mathf.Lerp(minSizeMult, maxSizeMult, _health / maxHealth);
            transform.localScale = baseScale * mult;
        }
    }
    private float _health;

    public float healthGroth;
    public float baseHealth;

    public float minHealthToBlowAir;
    public float blowAirSpeed;
    public float boostMaxSpeed;
    public float blowAirDisableTime;
    public float airLossPercent;
    public float airBubbleMinHealth;
    private float airBubbleCounter = 0;
    private float blowAirDisableTimer;
    private Vector2 boostVelocity;
    public float boostAccelertation;
    public float boostDeccelertation;

    public float invurnerableTime = 1f;
    private float invurnerableTimer = 0f;
    public float pushBackForce = 10f;


    public float upSpeed;
    public float horizontalMaxSpeed;
    public float horizontalMinSpeed;
    public float horizontalAcceleration;
    public float horizontalDeceleration;
    public float rotationSpeed;
    public Rigidbody2D rb;
    private CameraMovement camMovement;
    

    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    public KeyCode lookRightKey = KeyCode.RightArrow;
    public KeyCode lookDownKey = KeyCode.DownArrow;
    public KeyCode lookLeftKey = KeyCode.LeftArrow;
    public KeyCode lookUpKey = KeyCode.UpArrow;

    public KeyCode blowAirKey = KeyCode.Space;

    private Vector2 currentPos = Vector2.zero;
    private Vector2 moveVec = Vector2.zero;
    private Vector3 playerStartPos = Vector3.zero;
    private float horizontalMove = 0f;
    private Vector3 baseScale;
    private Vector3 direction = Vector3.up;

    [SerializeField] private GameObject blowBubble;
    [SerializeField] private ParticleSystem blowParticles;
    [SerializeField] public Animation anim;
    [SerializeField] public Animator animator;

    private float _maxHealth;
    private float _healthGroth;
    private float _horizontalAcceleration;
    private float _horizontalMaxSpeed;
    private float _airLossPercent;
    private float _blowAirSpeed;
    private float _boostAccelertation;
    private float _boostMaxSpeed;
    private float _rotationSpeed;

    [Header("Sound References")]
    [SerializeField] EventReference audio_playerDeath;
    [SerializeField] EventReference audio_collision;

    //private bool controle = true;

    private Enemy enemyS;

    private void Awake()
    {
        camMovement = Camera.main.GetComponent<CameraMovement>();
        _maxHealth = maxHealth;
        _healthGroth = healthGroth;
        _horizontalAcceleration = horizontalAcceleration;
        _horizontalMaxSpeed = horizontalMaxSpeed;
        _airLossPercent = airLossPercent;
        _blowAirSpeed = blowAirSpeed;
        _boostAccelertation = boostAccelertation;
        _boostMaxSpeed = boostMaxSpeed;
        _rotationSpeed = rotationSpeed;
        Deactivate();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Health -= 10f;
        //}
        GameManager.Instance.ButtonCalls.Points += pointsPerSecond * Time.deltaTime;
        if (blowAirDisableTimer > 0)
        {
            blowAirDisableTimer -= Time.deltaTime;
        }
        if (invurnerableTimer > 0)
        {
            invurnerableTimer -= Time.deltaTime;
        }

        //if (Input.GetKeyUp(blowAirKey))
        //{
        //    blowParticles.Stop();
        //}

        Grow();
        LookDirection();
    }

    public void Deactivate()
    {
        animator.enabled = false;
        anim.enabled = true;
        enabled = false;
        rb.simulated = false;
    }

    IEnumerator TriggerIdleAnim()
    {
        animator.Play("bubble_idle");
        yield return null;
        animator.Play("bubble_idle");
        yield return null;
        animator.Play("bubble_idle");
        yield return null;
        animator.Play("bubble_idle");
        yield return null;
        animator.Play("bubble_idle");
        yield return null;
    }

    public void Activate()
    {
        
        animator.enabled = true;
        anim.enabled = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(TriggerIdleAnim());
        enabled = true;
        rb.simulated = true;
        baseScale = transform.localScale;

        maxHealth = _maxHealth * GameManager.Instance.ButtonCalls.healthMults[GameManager.Instance.ButtonCalls.healthLevel];
        healthGroth = _healthGroth * GameManager.Instance.ButtonCalls.regenMults[GameManager.Instance.ButtonCalls.regenLevel];
        horizontalAcceleration = _horizontalAcceleration * GameManager.Instance.ButtonCalls.moveSpeedMults[GameManager.Instance.ButtonCalls.moveSpeedLevel];
        horizontalMaxSpeed = _horizontalMaxSpeed * GameManager.Instance.ButtonCalls.moveSpeedMults[GameManager.Instance.ButtonCalls.moveSpeedLevel];
        rotationSpeed = _rotationSpeed * GameManager.Instance.ButtonCalls.moveSpeedMults[GameManager.Instance.ButtonCalls.moveSpeedLevel];
        airLossPercent = _airLossPercent * GameManager.Instance.ButtonCalls.airLossMults[GameManager.Instance.ButtonCalls.airLossLevel];
        blowAirSpeed = _blowAirSpeed * GameManager.Instance.ButtonCalls.airLossMults[GameManager.Instance.ButtonCalls.airLossLevel];
        boostAccelertation = _boostAccelertation * GameManager.Instance.ButtonCalls.boostMults[GameManager.Instance.ButtonCalls.boostLevel];
        boostMaxSpeed = _boostMaxSpeed * GameManager.Instance.ButtonCalls.boostMults[GameManager.Instance.ButtonCalls.boostLevel];

        Health = baseHealth * maxHealth;
    }

    private void LookDirection()
    {
        Vector3 nextDirection = Vector3.zero;
        if (Input.GetKey(lookRightKey))
        {
            nextDirection += Vector3.right;
        }
        if (Input.GetKey(lookDownKey))
        {
            nextDirection += Vector3.down;
        }
        if (Input.GetKey(lookLeftKey))
        {
            nextDirection += Vector3.left;
        }
        if (Input.GetKey(lookUpKey))
        {
            nextDirection += Vector3.up;
        }
        if (nextDirection == Vector3.zero)
        {
            return;
        }
        direction = nextDirection;

        float currentRotation = rb.rotation;
        float targetRotation = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
        if (targetRotation < 0) { targetRotation = (180 - Mathf.Abs(targetRotation)) + 180; }
        float rotationDiff = targetRotation - currentRotation;
        if (Mathf.Abs(rotationDiff) < 3)
        {
            return;
        }
        int sign = 1;
        if ((rotationDiff < 0 || rotationDiff > 180) && rotationDiff > -180)
        {
            sign = -1;
        }
        rb.rotation = currentRotation + sign * rotationSpeed * Time.deltaTime;
        if (rb.rotation < 0) {  rb.rotation += 360; }
        else if (rb.rotation > 360) { rb.rotation -= 360;}
    }

    private void Grow()
    {
        Health += healthGroth * Time.deltaTime;
    }

    private void Movement()
    {
        currentPos = transform.position;
        moveVec = Vector2.zero;

        moveVec += BlowAir();
        moveVec += DefaultMovement();
        moveVec += ControledMovement();


        rb.MovePosition(currentPos + moveVec);
    }

    private Vector2 DefaultMovement()
    {
        Bounds bounds = camMovement.OrthographicBounds();
        Vector2 horizontalCorrection = Vector2.zero;
        if (transform.position.x > camMovement.transform.position.x + bounds.extents.x)
        { 
            horizontalCorrection = new Vector2(camMovement.transform.position.x + bounds.extents.x, transform.position.y) - rb.position; 
        }
        else if (transform.position.x < camMovement.transform.position.x - bounds.extents.x)
        {
            horizontalCorrection = new Vector2(camMovement.transform.position.x - bounds.extents.x, transform.position.y) - rb.position;
        }

        if (transform.position.y < camMovement.transform.position.y - bounds.extents.y) { Kill(); }

        
        return Vector2.up * upSpeed + horizontalCorrection;

    }

    private Vector2 ControledMovement()
    {
        int horizontalDirection = 0;
    
        if (Input.GetKey(moveRightKey)) 
        { 
            horizontalDirection += 1; 
      
        }
        if (Input.GetKey(moveLeftKey)) 
        { 
            horizontalDirection -= 1; 
        }

        if (horizontalDirection == 0) 
        {
            horizontalMove *= horizontalDeceleration;
            if (Mathf.Abs(horizontalMove) < horizontalMinSpeed) { horizontalMove = 0; } 
        }
        else
        {
            horizontalMove += horizontalDirection * horizontalAcceleration;
            horizontalMove = Mathf.Abs(horizontalMove) > horizontalMaxSpeed ? horizontalMaxSpeed * horizontalDirection : horizontalMove;
        }

        return Vector2.right * horizontalMove;
    }

    private Vector2 BlowAir()
    {
        if (blowAirDisableTimer > 0 || !Input.GetKey(blowAirKey))
        {
            blowParticles.Stop();
            boostVelocity *= boostDeccelertation;
            return boostVelocity;
        }
        if (Health <= minHealthToBlowAir)
        {
            blowParticles.Stop();
            blowAirDisableTimer = blowAirDisableTime;
            return boostVelocity;
        }

        if (!blowParticles.isPlaying) { blowParticles.Play(); }
        Health -= blowAirSpeed;

        Vector2 boostDir = Quaternion.AngleAxis(rb.rotation, Vector3.back) * Vector2.up;
        boostDir = new Vector2(boostDir.x, -boostDir.y);

        boostVelocity += boostDir * boostAccelertation;
        if (boostVelocity.magnitude > boostMaxSpeed)
        {
            boostVelocity = boostVelocity.normalized * boostMaxSpeed;
        }

        airBubbleCounter += blowAirSpeed * (1f - airLossPercent);
        if (airBubbleCounter >= airBubbleMinHealth)
        {
            airBubbleCounter = 0;

            Vector3 bubbleSpawnPos = transform.position + new Vector3(-boostDir.x, -boostDir.y, 0) * transform.localScale.x * 2.5f;
            GameObject bubble = Instantiate(blowBubble, bubbleSpawnPos, Quaternion.identity);
            BlowBubble bubbleBehavoiur = bubble.GetComponent<BlowBubble>();

            bubbleBehavoiur.velocity = -boostVelocity;
            bubbleBehavoiur.Health = airBubbleMinHealth;
        }

        return boostVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invurnerableTimer > 0) { return; }
        if (collision.gameObject.tag != "Enemy") { return; }
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemyS = enemy; //reference to the enemy that the player hits
        if (enemy == null || enemy.DamageOnCollision == 0) { return; }
        Health -= enemy.DamageOnCollision;
        invurnerableTimer = invurnerableTime;
        //Vector2 forceDir = transform.position - collision.transform.position;
        Vector2 forceDir = transform.position - collision.transform.position;
        forceDir = forceDir.normalized;
        StartCoroutine(PushAfterEnemyHit(forceDir));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (invurnerableTimer > 0) { return; }
        if (collision.gameObject.tag != "Enemy") { return; }
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy == null || enemy.DamageOnCollision == 0) { return; }
        if (enemy != null) { PlayCollisionSound(); }
        invurnerableTimer = invurnerableTime;
        Health -= enemy.DamageOnCollision;
        Vector2 forceDir = transform.position - collision.transform.position;
        forceDir = forceDir.normalized;
        StartCoroutine(PushAfterEnemyHit(forceDir)); 
    }

    IEnumerator PushAfterEnemyHit(Vector2 direction)
    {
        float timer = 0.1f;
        while (true) 
        {
            rb.AddForce(direction * pushBackForce);
            timer -= Time.deltaTime;
            if (timer < 0) { break; }
            yield return new WaitForEndOfFrame();
        }
    }

    public void Kill()
    {
        if (isImune) { return; }
        StartCoroutine(PlayDeathAnim());
    }

    IEnumerator PlayDeathAnim()
    {

        animator.Play("bubble_death");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        GameManager.Instance.StartCoroutine(dothething());
        enemyS.PlayEnemySound();
        PlayerDeathSound();
        GameManager.Instance.SwitchToGameOver();
    }

    private void PlayerDeathSound()
    {
        if (audio_playerDeath.Path.Length > 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(audio_playerDeath);
        }
        else
        {
            Debug.Log("Player Death Anim Sound Reference Not Assigned.");
        }
    }

    private void PlayCollisionSound()
    {
        if (audio_collision.Path.Length > 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(audio_collision);
        }
        else
        {
            Debug.Log("Player Death Anim Sound Reference Not Assigned.");
        }
    }

    private IEnumerator dothething()
    {
        yield return new WaitForSeconds(5.0f);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        animator.Play("bubble_idle");
    }
}
