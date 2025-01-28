using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamagable, IDataPersistence
{
    public static Player instance { get; private set; }

    public Rigidbody2D rb { get; private set; }

    [SerializeField] private Transform _followCameraPoint;
    public Transform followCameraPoint { get { return _followCameraPoint; } }

    [Header("Health")]
    [SerializeField] private int startMaxHealth = 200;
    int _maxHealth;
    public int maxHealth
    {
        get => _maxHealth;
        set
        {
            int dif = value - _maxHealth;
            _maxHealth = value;
            health += dif;
            MainUIM.instance.baseStates.SetMaxHealth(_maxHealth);
        }
    }

    private int _health;
    public int health
    {
        get { return _health; }
        set
        {
            if (value >= maxHealth)
                _health = maxHealth;
            else if (value <= 0)
                _health = 0;
            else
                _health = value;
        }
    }

    [Header("Stamina")]
    [SerializeField] private int startMaxStamina;
    float _maxStamina;
    public float maxStamina { get => _maxStamina;
        set
        {
            float dif = value - _maxStamina;
            _maxStamina = value;
            stamina += dif;
            MainUIM.instance.baseStates.SetMaxStamina(_maxStamina);
        }
    }
    public float staminaRecoverySpeed = 1f;
    float _stamina;
    float stamina
    {
        get { return _stamina; }
        set
        {
            if (value <= 0)
            {
                _stamina = 0;
                canUseStamina = false;
                //StartCoroutine(StaminaZero());
            }
            else if (value >= maxStamina)
            {
                _stamina = maxStamina;
                canUseStamina = true;
            }
            else
                _stamina = value;
        }
    }

    [Header("Movement")]
    public float normalSpeed = 3f;
    private float currentSpeed;
    private Vector2 _moveDirection;
    private bool move;
    public Vector2 moveDirection
    {
        get { return _moveDirection; }
        private set
        {
            if (value.magnitude != 0)
                move = true;
            else
                move = false;

            rb.velocity = value.normalized * currentSpeed;

            _moveDirection = value;
        }
    }

    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [Header("Runing")]
    [SerializeField] private float runSpeedModifier = 1.2f;
    [SerializeField] private float runBoostModifier = 1.2f;
    [SerializeField] private float timeToRunBoost = 1f;
    private float runBustBuffer = 0f;
    [SerializeField] private float runStaminaWaste = 1;
    public bool runing { get; private set; }

    [Header("Damage Object With Run Boost")]
    [SerializeField] private bool enableDamageObjectWhileBoost = true;
    [SerializeField] private int bodyDamage = 30;

    [Header("Dashing")]
    public float dashForce;
    public float dashingTime;
    [SerializeField] private int staminaToDash;
    private float _dashingTimeBuffer;
    private float dashingTimeBuffer
    {
        get { return _dashingTimeBuffer; }
        set
        {
            if (value <= 0)
            {
                _dashingTimeBuffer = 0;
                dashing = false;
                dashTrail.emitting = false;
            }
            else if (value >= dashingTime)
            {
                _dashingTimeBuffer = dashingTime;
                dashing = true;
                dashTrail.emitting = true;
            }
            else
            {
                _dashingTimeBuffer = value;
                dashing = true;
                dashTrail.emitting = true;
            }
        }
    }
    private bool dashing;

    [SerializeField] private float timeToStaminaUp = 0.1f;
    private float timeToStaminaUpBuffer;

    bool _canUseStamina = true;
    bool isHeal = false;

    // ���� ��������� � ������� ������� � � �������� ����� ������� � ��������� ������!!
    [Header("Grenade")]
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float forceGrenade = 300f;

    [Header("HealthBottle")]
    [SerializeField] private int hpToHeal = 10;
    [SerializeField] private float timeToHeal = 2f;

    bool canUseStamina
    {
        get { return _canUseStamina; }
        set
        {
            MainUIM.instance.baseStates.SetCanUseStamina(value);
            // HPStaminaManager.instance.canUseStamina = value;
            _canUseStamina = value;
        }
    }

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform secondAttackPoint;
    [SerializeField] private LayerMask hideObjectLayer;
    public float attackRange = 0.5f;
    public float attackCooldown = 2f;
    float nextAttackTime = 0f;
    public float seriesAttackTime = 1f;
    float seriesAttack = 0f;
    public int damage = 5;
    [SerializeField] private bool autoReloading;

    public bool attack { get; private set; }

    int punchSide = 1;
    
    public Vector2 attackPointPosition { get { return attackPoint.localPosition; } }

    [Header("Reloading")]
    [SerializeField] private bool autoreloading;

    [Header("Animations")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    private Animator legsAnimator;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private TrailRenderer dashTrail;

    [Header("Sound Effects")]
    [SerializeField] private GameObject punchSound;
    [SerializeField] private GameObject damageSound;
    [SerializeField] private SoundEffect dashSE;
    [SerializeField] private SoundEffect grenadeThrowSE;
    [SerializeField] private SoundEffect healSE;

    [Header("Physics")]
    [SerializeField] private Rigidbody2D _rb;

    private bool _isOnBattle = false; // �������� ��������� � ������ ������� ��� ������

    [HideInInspector] public Vector2 startPosition; // ��� ��� ���� ��������� � ���������� ������
    [HideInInspector] public string levelName; // �������� �� ����� ��������� � ������ ������

    [Header("Setup")]
    [SerializeField] private PlayerWeaponsManager _weaponsManager;
    public PlayerWeaponsManager weaponsManager { get { return _weaponsManager; } private set { _weaponsManager = value; } }

    [SerializeField] private InteractableObjectsDetector _interactableObjectsDetector;
    public InteractableObjectsDetector interactableObjectsDetector { get { return _interactableObjectsDetector; } }

    private void OnValidate()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
        if (bodyAnimator == null)
            bodyAnimator = GetComponent<Animator>();
        if (dashTrail == null)
            dashTrail = GetComponentInChildren<TrailRenderer>();
        if (_interactableObjectsDetector == null)
            _interactableObjectsDetector = GetComponentInChildren<InteractableObjectsDetector>();
        if (_weaponsManager == null)
        {
            _weaponsManager = GetComponentInChildren<PlayerWeaponsManager>();
            weaponsManager.player = this;
        }
    }

    private void Awake()
    {
        if (instance != null) Debug.LogWarning("Find more than one Player script in scene");
        instance = this;

        health = maxHealth;
        stamina = maxStamina;

        currentSpeed = normalSpeed;

        rb = _rb;
        legsAnimator = legs.GetComponent<Animator>();

        dashTrail.emitting = false;

        CursorManager.instance.InvokeUpdateAimPosition();
        PlayerConditionsManager.instance.currentCondition = PlayerConditions.Default;
    }

    private void Start()
    {
        maxHealth = startMaxHealth;
        maxStamina = startMaxStamina;
    }


    private void Update()
    {
        Runing();

        if (!dashing)
            moveDirection = InputManager.instance.moveDirection;

        if (InputManager.instance.GetInteractPressed() && _interactableObjectsDetector.interactable != null)
            _interactableObjectsDetector.interactable.Interact(this);


        if (!isHeal)
        {
            Attack();
        }

        Animate();
        UpdateUI(); // �������� ������ ������ ����
        RotatePlayer();
        RotatePlayersLegs();
        CountTimeVariables();
    }

    private void OnEnable()
    {
        Invoke("OnEnableAfterTime", 0.1f); 
    }

    private void OnEnableAfterTime()
    {
        GameEventsManager.instance.playerWeapons.onWeaponChanged += AnimateChangedWeapon;
        GameEventsManager.instance.input.onDashPressed += Dash;
        GameEventsManager.instance.input.onReloadPressed += ReloadGun;

        GameEventsManager.instance.input.onGrenadeAttack += UseGrenade;
        GameEventsManager.instance.input.onHealthBottle += UseHealth;

        GameEventsManager.instance.input.onMapPressed += OpenCloseMap;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= AnimateChangedWeapon;
        GameEventsManager.instance.input.onDashPressed -= Dash;
        GameEventsManager.instance.input.onReloadPressed -= ReloadGun;

        GameEventsManager.instance.input.onGrenadeAttack -= UseGrenade;
        GameEventsManager.instance.input.onHealthBottle -= UseHealth;

        GameEventsManager.instance.input.onMapPressed -= OpenCloseMap;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enableDamageObjectWhileBoost
            && runBustBuffer >= timeToRunBoost /*��������� ���� � ��������*/
            && collision.gameObject.CompareTag("Object"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(bodyDamage, this.transform);
            }
        }
    }

    private IEnumerator OnLevelWasLoaded(int level)
    {
        yield return new WaitForSecondsRealtime(0.01f);

        if (PlayerDataKeeper.instance.playerData != null)
            LoadPlayerData();
    }

    public void SavePlayerData(Vector2 nextLevelStartPosition)
    {
        PlayerData newData = new PlayerData();

        newData.maxHealth = maxHealth;
        newData.maxStamina = maxStamina;
        newData.heath = health;
        newData.nextLevelStartPosition = nextLevelStartPosition;

        newData.lightBullets = weaponsManager.GetAmmoByType(AmmoTypes.LightBullets);
        newData.mediumBullets = weaponsManager.GetAmmoByType(AmmoTypes.MediumBullets);
        newData.heavyBullets = weaponsManager.GetAmmoByType(AmmoTypes.HeavyBullets);
        newData.shells = weaponsManager.GetAmmoByType(AmmoTypes.Shells);

        // newData.weapons = PlayerWeaponsManager.instance.weapons;
        newData.gun1 = weaponsManager.firstSlotGun;
        newData.gun2 = weaponsManager.secondSlotGun;
        newData.gun3 = weaponsManager.thirdSlotGun;
        newData.meleeWeapon = weaponsManager.slotMeleeWeapon;

        newData.moneyCount = PlayerInventory.instance.money;
        newData.voidBottleCount = PlayerInventory.instance.countEmptyBottle;
        newData.demonsBloodGrenadeCount = PlayerInventory.instance.countGrenadeBottle;
        newData.healthPoitionCount = PlayerInventory.instance.countHealthBottle;

        PlayerDataKeeper.instance.playerData = newData;
    }

    void LoadPlayerData()
    {
        PlayerData dataToLoad = PlayerDataKeeper.instance.playerData;

        maxStamina = dataToLoad.maxStamina;
        maxHealth = dataToLoad.maxHealth;
        health = dataToLoad.heath;
        transform.position = dataToLoad.nextLevelStartPosition;

        weaponsManager.SetAmmoByType(AmmoTypes.LightBullets, dataToLoad.lightBullets);
        weaponsManager.SetAmmoByType(AmmoTypes.MediumBullets, dataToLoad.mediumBullets);
        weaponsManager.SetAmmoByType(AmmoTypes.HeavyBullets, dataToLoad.heavyBullets);
        weaponsManager.SetAmmoByType(AmmoTypes.Shells, dataToLoad.shells);

        // PlayerWeaponsManager.instance.weapons = dataToLoad.weapons;
        weaponsManager.firstSlotGun = dataToLoad.gun1;
        weaponsManager.secondSlotGun = dataToLoad.gun2;
        weaponsManager.thirdSlotGun = dataToLoad.gun3;
        weaponsManager.slotMeleeWeapon = dataToLoad.meleeWeapon;

        PlayerInventory.instance.money = dataToLoad.moneyCount;
        PlayerInventory.instance.countEmptyBottle = dataToLoad.voidBottleCount;
        PlayerInventory.instance.countGrenadeBottle = dataToLoad.demonsBloodGrenadeCount;
        PlayerInventory.instance.countHealthBottle = dataToLoad.healthPoitionCount;

        PlayerDataKeeper.instance.ClearData();
    }

    public void LoadData(GameData data)
    {
        this.health = data.playerHealth;

        this.startPosition = data.startPosition;
        this.levelName = data.levelName;

        if (levelName == SceneManager.GetActiveScene().name)
            transform.position = this.startPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerHealth = this.health;
        data.startPosition = this.startPosition;
        data.levelName = this.levelName;
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        if (dashing)
            return;

        health -= damage;

        EffectsManager.instance.PlaySoundEffect(damageSound, 2f, 0.8f, 1.2f);

        if (attack != null)
        {
            var effect = Instantiate(damageEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), attack.rotation);
            Destroy(effect, 1f);
        }

        if (health <= 0)
        {
            PlayerDeath();
        }
    }

    public void TakeDamageAlways(int damage, Transform attack = null) // �������� ���� ������ ���������� �� ���������
    {
        health -= damage;

        EffectsManager.instance.PlaySoundEffect(damageSound, 2f, 0.8f, 1.2f);

        if (attack != null)
        {
            var effect = Instantiate(damageEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), attack.rotation);
            Destroy(effect, 1f);
        }

        if (health <= 0)
        {
            PlayerDeath();
        }
    }

    private void Runing()
    {
        if (InputManager.instance.GetRunPressed(true) && canUseStamina && !isHeal && !attack && !weaponsManager.reloadingProccess)
        {
            if (runBustBuffer >= timeToRunBoost) // ��������� ���� ��������
            {
                bodyAnimator.SetFloat("Run Multiplier", runBoostModifier);
                currentSpeed = normalSpeed * runSpeedModifier * runBoostModifier;
                dashTrail.emitting = true;
            }
            else // ���� �������� �� ���������
            {
                bodyAnimator.SetFloat("Run Multiplier", 1f);
                currentSpeed = normalSpeed * runSpeedModifier;
                dashTrail.emitting = false;
            }
            

            if (runing == false)
            {
                bodyAnimator.SetTrigger("Run");
            }

            runing = true;

            if (moveDirection != Vector2.zero)
                stamina -= runStaminaWaste * Time.deltaTime;
        }
        else
        {
            if (runing == true)
            {
                bodyAnimator.SetTrigger("Change Weapon");
            }

            runing = false;

            currentSpeed = normalSpeed;
            dashTrail.emitting = false;
        }
    }

    private void PlayerDeath()
    {
        health = 0;

        GameEventsManager.instance.player.PlayerDeath();

        Destroy(gameObject, 0.1f);
    }

    private void Animate()
    {
        bodyAnimator.SetFloat("Punch Side", punchSide);
        bodyAnimator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        if (weaponsManager.currentWeapon != null) bodyAnimator.SetInteger("Weapon ID", weaponsManager.currentWeapon.id);
        else bodyAnimator.SetInteger("Weapon ID", 0);

        if (move)
            legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
        else
            legsAnimator.SetFloat("Speed", 0);
    }

    private void AnimateChangedWeapon()
    {
        if (!runing)
            bodyAnimator.SetTrigger("Change Weapon");
    }

    bool rotateOnAim;
    private void RotatePlayer()
    {
        if (runing || dashing)
        {
            Vector3 rotation = legs.transform.eulerAngles;
            rb.transform.localEulerAngles = legs.transform.eulerAngles;
            legs.transform.eulerAngles = rotation;

            rotateOnAim = false;
        }
        else
        {
            Vector2 lookDirection;

            lookDirection = InputManager.instance.lookDirection;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            rb.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            rotateOnAim = true;
        }
    }

    private void RotatePlayersLegs()
    {
        Vector2 moveDir = InputManager.instance.moveDirection.normalized;

        if (moveDir == Vector2.zero)
        {
            legs.transform.localRotation = Quaternion.identity;
            return;
        }

        float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(legs.transform.eulerAngles.z, targetAngle - 90, ref turnSmoothVelocity, turnSmoothTime);

        legs.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Attack()
    {
        if (runing)
           return;

        if (weaponsManager.currentWeapon == null
            && nextAttackTime <= 0 && InputManager.instance.GetAttackPressed()) // ����� ���������
        {
            InputManager.instance.ReroizeAttackBuffer();

            if (seriesAttack <= 0) { punchSide = 0; bodyAnimator.SetFloat("Punch Side", punchSide); }

            bodyAnimator.SetTrigger("Attack");
            attack = true;

            var soundEffect = Instantiate(punchSound);
            soundEffect.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            Destroy(soundEffect, attackCooldown);

            Collider2D[] hitDamagedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

            foreach (var obj in hitDamagedObjects)
            {
                if (obj.gameObject == this.gameObject)
                    continue;

                IDamagable damagedObj = obj.GetComponent<IDamagable>();

                if (damagedObj != null) damagedObj.TakeDamage(damage, transform);

            }

            if (punchSide == 0) { punchSide = 1; }
            else if (punchSide == 1) { punchSide = 0; }

            seriesAttack = seriesAttackTime;

            nextAttackTime = attackCooldown;
        }
        else if (weaponsManager.currentWeapon != null
            && InputManager.instance.GetAttackPressed(weaponsManager.currentWeapon.holdToAttack)
            && weaponsManager.currentWeaponCooldown <= 0
            && !weaponsManager.reloadingProccess
            && rotateOnAim) // ����� �������
        {
            InputManager.instance.ReroizeAttackBuffer();

            if (weaponsManager.currentGun.ammoInMagazine > 0)
            {
                bodyAnimator.SetFloat("Attack Multiplier", weaponsManager.currentGun.firingRate); // ����� ������ ��� ������������� ������ �������� ���!!!
                bodyAnimator.SetTrigger("Attack");
                attack = true;
            }

            Transform currentAttackPoint = null;

            if (CheckObstacles(attackPoint.localPosition.y, hideObjectLayer))
            {
                currentAttackPoint = secondAttackPoint;
            }
            else
            {
                currentAttackPoint = attackPoint;
            }

            if (attackPoint != null)
            {
                weaponsManager.currentWeapon.Attack(currentAttackPoint);

                if (weaponsManager.currentGun.ammoInMagazine == 0)
                {
                    if (weaponsManager.GetAmmoByType(weaponsManager.currentGun.ammoType) > 0
                        && autoreloading)
                    {
                        ReloadGun();
                        attack = false;
                    }
                    weaponsManager.CheckAmmo();
                    attack = false;
                }
            }
        }
        else
        {
            attack = false;
        }
    }

    private void OpenCloseMap()
    {
        if (MiniMapUIM.instance.mapEnable)
            MiniMapUIM.instance.HideMiniMap();
        else
            MiniMapUIM.instance.ShowMiniMap();
    }

    private void Dash()
    {
        if (canUseStamina && stamina >= staminaToDash && !weaponsManager.reloadingProccess && !isHeal)
        {
            if (InputManager.instance.moveDirection.magnitude != 0)
            {
                rb.AddForce(InputManager.instance.moveDirection * dashForce * rb.mass, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.up * dashForce * rb.mass, ForceMode2D.Impulse);
            }

            bodyAnimator.Play("Dash");

            AudioManager.instance.PlaySoundEffect(dashSE, dashingTime);

            dashingTimeBuffer = dashingTime;
            stamina = 0;
        }
    }

    private void DashEnd()
    {
        bodyAnimator.SetTrigger("Change Weapon");
        timeToStaminaUpBuffer = 0;
    }

    public bool CheckObstacles(float distance, LayerMask layer)
    {
        var hits = Physics2D.RaycastAll(rb.position, InputManager.instance.lookDirection, distance);

        foreach(var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.layer == layer)
                continue;
            return true;
        }
        return false;
    }

    private void ReloadGun()
    {
        if (runing)
            return;

        weaponsManager.Reload();     
    }

    public void PlayReloadingAnimation()
    {
        bodyAnimator.SetFloat("Reloading Multiplier", 1 / weaponsManager.currentGun.reloadTime);
        bodyAnimator.SetTrigger("Reload");
    }

    private void CountTimeVariables()
    {
        if (nextAttackTime > 0)
            nextAttackTime -= Time.deltaTime;

        if (seriesAttack > 0)
            seriesAttack -= Time.deltaTime;

        if (dashingTimeBuffer > 0)
        {
            dashingTimeBuffer -= Time.deltaTime;
            if (dashingTimeBuffer <= 0)
                DashEnd();
        }

        if (stamina != maxStamina && timeToStaminaUpBuffer <= 0)
            stamina += Time.deltaTime * staminaRecoverySpeed;

        if (runing && moveDirection != Vector2.zero)
        {
            timeToStaminaUpBuffer = timeToStaminaUp;
        }

        if (timeToStaminaUpBuffer > 0)
        {
            timeToStaminaUpBuffer -= Time.deltaTime;
            runBustBuffer += Time.deltaTime;
        }
        else
        {
            runBustBuffer = 0;
        }
            
    }

    private void UpdateUI()
    {
        MainUIM.instance.baseStates.SetCurrentHealth(health);

        MainUIM.instance.baseStates.SetCurrentStamina(stamina);
    }

    private void UseGrenade()
    {
        if (runing)
            return;

        if (PlayerInventory.instance.countGrenadeBottle > 0)
        {
            if (grenadeThrowSE != null)
                AudioManager.instance.PlaySoundEffect(grenadeThrowSE, transform.position, 2f);

            Vector3 bulletAngle = followCameraPoint.eulerAngles;
            GameObject grenade = Instantiate(grenadePrefab, followCameraPoint.position, Quaternion.Euler(bulletAngle));
            Rigidbody2D grb = grenade.GetComponent<Rigidbody2D>();
            grb.AddForce(grenade.transform.up * forceGrenade, ForceMode2D.Impulse);
            PlayerInventory.instance.countGrenadeBottle--;
        }
    }

    private void UseHealth()
    {
        if (runing)
            return;

        if (PlayerInventory.instance.countHealthBottle > 0 && health < maxHealth)
        {
            bodyAnimator.SetFloat("Healing Multiplier", 1 / timeToHeal);
            StartCoroutine("HealthBottleDrinkCouroutine");
            PlayerInventory.instance.countHealthBottle--;
            PlayerInventory.instance.countEmptyBottle++;
        }
    }

    private IEnumerator HealthBottleDrinkCouroutine()
    {
        isHeal = true;

        bodyAnimator.SetTrigger("Healing");

        yield return new WaitForSeconds(timeToHeal);

        instance.health += hpToHeal;

        isHeal = false;

        bodyAnimator.SetTrigger("Change Weapon");
    }

    void PlayHealingSE()
    {
        AudioManager.instance.PlaySoundEffect(healSE, timeToHeal);
    }

    private IEnumerator StaminaZero()
    {
        canUseStamina = false;
        while (stamina < maxStamina)
        {
            yield return new WaitForEndOfFrame();
        }
        canUseStamina = true;
    }
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null) return; 

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
