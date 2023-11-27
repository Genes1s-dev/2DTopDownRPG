using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance;
    [SerializeField] private List<BaseWeapon> weapons;
    private BaseWeapon currentWeapon;
    [SerializeField] private GameInput gameInput;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    private PlayerStats playerStats;


    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector3 aimPosition;
    private Vector2 lastMoveInput = Vector2.down;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 10.0f;
    private float initialMoveSpeed;
    [SerializeField] private float arrowFlightSpeed = 15.0f;
    [SerializeField] private float moveSpeedPenalty = 2.0f;
    [SerializeField] private float arrowShootCooldown = 1.5f;
    private bool isCooldown = false;
    private bool isWalking = false;
    //[HideInInspector] public bool isAiming = false;
    //[HideInInspector] public bool isCharging = false;
    
    public bool isAiming {get; private set;}
    public bool isCharging {get; private set;}
    [Header("PlayerStuff")]
    [SerializeField] private GameObject light;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject bowAndArrow;

    [Header("Items")]
    //[HideInInspector] public bool hasKey;
    private List<GameObject> items;
    private List<RaycastHit2D> hitList = new List<RaycastHit2D>();
    private Inventory inventory;



    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
        } else {
            Instance = this;
        }

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        currentWeapon = weapons[0];
        initialMoveSpeed = moveSpeed;

        gameInput.OnAimingStarted += GameInput_OnAimingStarted;
        gameInput.OnAimingCanceled += GameInput_OnAimingCanceled;
        gameInput.OnWeaponSwitchPerformed += GameInput_OnWeaponSwitchPerformed;
        gameInput.OnInteractPerformed += GameInput_OnInteractPerformed;
    }

    private void GameInput_OnAimingStarted(object sender, EventArgs e)
    {
        switch(currentWeapon.WeaponType)
        {
            case BaseWeapon.Type.sword:
                currentWeapon.Hit();
                break;
            case BaseWeapon.Type.bow:
                if (!currentWeapon.IsCooldown)
                {
                    isAiming = true;
                    moveSpeed *= currentWeapon.GetWeaponSO().moveSpeedPenaltyCoef;
                }
                break;
            case BaseWeapon.Type.magic:
                if (!currentWeapon.IsCooldown)
                {
                    isCharging = true;
                    moveSpeed *= currentWeapon.GetWeaponSO().moveSpeedPenaltyCoef;
                }
                break;
            default:
                break;
        }  
    }



    private void GameInput_OnAimingCanceled(object sender, EventArgs e)
    {
        switch(currentWeapon.WeaponType)
        {
            case BaseWeapon.Type.sword:
                break;
            case BaseWeapon.Type.bow:
                if (!currentWeapon.IsCooldown && isAiming)
                {
                    currentWeapon.Hit();
                    isAiming = false;
                    moveSpeed = initialMoveSpeed;
                }
                break;
            case BaseWeapon.Type.magic:
                if (!currentWeapon.IsCooldown && isCharging)
                {
                    SpendEnergy(currentWeapon.GetEnergyConsumption());//?
                    currentWeapon.Hit();
                    isCharging = false;
                    moveSpeed = initialMoveSpeed;
                }
                break;
            default:
                break;
        }
    }

    private void GameInput_OnWeaponSwitchPerformed(object sender, EventArgs e)
    {
        if (!isAiming && !currentWeapon.IsCooldown)
        {
            if (weapons.IndexOf(currentWeapon) >= weapons.Count - 1)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weapons[0];
                currentWeapon.gameObject.SetActive(true);
            } else {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weapons[weapons.IndexOf(currentWeapon) + 1];
                currentWeapon.gameObject.SetActive(true);
            }
        }
    }

    private void GameInput_OnInteractPerformed(object sender, EventArgs e)
    {
        //CircleCast, foreach cycle, only iinteractible to consider
        float radius = 0.09f;
        //RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, radius, GetLastMoveInput().normalized, detectionDistance);
        //Метод Physics.RaycastAllNonAlloc предназначен для ситуаций, когда требуется выполнить большое количество лучевых трассировок в цикле или во время обновления кадра. 
        //Он позволяет избежать аллокации памяти при каждом вызове функции, предварительно выделяя массив и передавая его в качестве аргумента.
        //Однако, в данном случае, мы ищем только первый объект, удовлетворяющий условию интерфейса IInteractable, и после этого прекращаем дальнейший поиск. 
        //Нам НЕ требуется хранить и обрабатывать все столкновения луча с объектами. Поэтому использование Physics.RaycastAllNonAlloc с предварительно выделенным массивом было бы излишним.
        //Вместо этого, используя Physics.RaycastAll, мы можем прервать цикл после взаимодействия с первым подходящим объектом, что эффективно и избавляет нас от необходимости создания и управления предварительно выделенным массивом.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, 0.05f, 0), radius);


        float closestDistance = Mathf.Infinity;
        Collider2D closestCollider = null;

        foreach (Collider2D collider in colliders)
        {
  
            if (collider.CompareTag("Player"))
            {
                continue;
            } 


            // Вычисляем расстояние до игрока
            float distance = Vector2.Distance(transform.position, collider.bounds.center);

            // Если найден ближайший объект
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }
 
        if (closestCollider != null)
        {
            IInteractable objectToInteract = closestCollider.gameObject.GetComponent<IInteractable>();
            if (objectToInteract != null)
            {
                objectToInteract?.Interact();
            }
        }
    }


    private void Update()
    {
        Animate();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    

    private void HandleMovement()
    {
        moveInput = gameInput.GetMovementVectorNormalized();

        if (moveInput != Vector2.zero)
        {
            lastMoveInput = moveInput;
        }

        if (moveSpeed != 0)
        {
            float moveDistance = moveSpeed * Time.fixedDeltaTime;
        
            //Получаем количество коллайдеров на нашем пути
            int collisionCount = rigidbody.Cast(moveInput, hitList, moveSpeed * Time.fixedDeltaTime * 0.05f);

            //При этом не учитывая коллайдеры-триггеры
            foreach(RaycastHit2D hit in hitList)
            {
                if (hit.collider.isTrigger)
                {
                    collisionCount--;
                }
            }

            //если кол-во коллайдеров нулевое, значит путь чист и мы можем передвигаться
            if (collisionCount == 0)
            {
                rigidbody.MovePosition(rigidbody.position + moveInput * moveDistance);
            } 
        }        
    } 


    private void Animate()
    {
        if (moveInput != Vector2.zero) //устанавливаем входные данные только если игрок нажимает клавиши. 
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
        }
        
        if (moveInput == Vector2.zero)
        {
            animator.SetFloat("Speed", 0f);
        } else {
            animator.SetFloat("Speed", 1f);
        }

        if (isAiming &&  !currentWeapon.IsCooldown)
        {
            animator.SetBool("isAiming", true);
        } else {
            animator.SetBool("isAiming", false);
        }

        if (isCharging &&  !currentWeapon.IsCooldown)
        {
            animator.SetBool("isCharging", true);
        } else {
            animator.SetBool("isCharging", false);
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }

    public Vector2 GetLastMoveInput()
    {
        return lastMoveInput;
    }

    public void TakeDamage(int amount, WeaponSO.Element element)
    {
        playerStats.UpdateHealth(-amount);
    }

    public void RestoreHealth(int amount)
    {
        playerStats.UpdateHealth(amount);
    }

    public void RestoreEnergy(float amount)
    {
        playerStats.UpdateEnergy(amount);
    }

    public void SpendEnergy(float amount)
    {
        playerStats.UpdateEnergy(-amount);
    }

    public void DeathSequence()
    {
        Debug.LogWarning("You died!!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Blackout"))
        {
            light.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Blackout"))
        {
            light.SetActive(false);
        }
    }
}
