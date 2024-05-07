using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable, IDataPersistence
{
    public static Player instance { get; private set; }

    public Rigidbody2D rb { get; private set; }

    [SerializeField] private Transform _followCameraPoint;
    public Transform followCameraPoint { get { return _followCameraPoint; } }

    [Header("Health")]
    public int maxHealth;

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

    [Header("Runing")]
    [SerializeField] private float runSpeedModifier = 1.2f;
    [SerializeField] private float runStaminaWaste = 1;

    [Header("Dashing")]
    public float dashForce;
    public float dashingTime;
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

    [Header("Stamina")]
    public float maxStamina = 1f;
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

    bool _canUseStamina = true;
    bool canUseStamina
    {
        get { return _canUseStamina; }
        set
        {
            HPStaminaManager.instance.canUseStamina = value;
            _canUseStamina = value;
        }
    }

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform secondAttackPoint;
    public float attackRange = 0.5f;
    public float attackCooldown = 2f;
    float nextAttackTime = 0f;
    public float seriesAttackTime = 1f;
    float seriesAttack = 0f;
    public int damage = 5;

    int punchSide = 1;

    public Vector2 attackPointPosition { get { return attackPoint.localPosition; } }

    [Header("Animations")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    private Animator legsAnimator;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private TrailRenderer dashTrail;

    [Header("Interactable Objects Detect")]
    [SerializeField] private InteractableObjectsDetector interactableObjectsDetector;

    [Header("Sound Effects")]
    [SerializeField] private GameObject punchSound;
    [SerializeField] private GameObject damageSound;
    [SerializeField] private SoundEffect dashSoundEffect;

    [Header("Physics")]
    [SerializeField] private Rigidbody2D _rb;

    private bool _isOnBattle = false; // параметр необходим в первую очередь для музыки
    public bool isOnBattle
    {
        get { return _isOnBattle; }
        set
        {
            if (value)
                AudioManager.instance.battleSnapshot.TransitionTo(2f);
            else
                AudioManager.instance.normalSnapshot.TransitionTo(0.5f);
            _isOnBattle = value;
        }
    }

    [HideInInspector] public Vector2 startPosition; // эти два поля относятся к сохранению игрока
    [HideInInspector] public string levelName; // возможно их стоит перенести в другой скрипт

    private void OnValidate()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
        if (bodyAnimator == null)
            bodyAnimator = GetComponent<Animator>();
        if (dashTrail == null)
            dashTrail = GetComponentInChildren<TrailRenderer>();
        if (interactableObjectsDetector == null)
            interactableObjectsDetector = GetComponentInChildren<InteractableObjectsDetector>();
            
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
    }

    private void Update()
    {
        if (InputManager.instance.GetRunPressed(true) && canUseStamina)
        {
            currentSpeed = normalSpeed * runSpeedModifier;
            dashTrail.emitting = true;

            if (moveDirection != Vector2.zero)
                stamina -= runStaminaWaste * Time.deltaTime;
        }
        else
        {
            currentSpeed = normalSpeed;
            dashTrail.emitting = false;
        }

        if (!dashing)
            moveDirection = InputManager.instance.moveDirection;

        if (InputManager.instance.GetInteractPressed() && interactableObjectsDetector.interactable != null)
            interactableObjectsDetector.interactable.Interact(this);

        Attack();

        Animate();
        UpdateUI(); // затратно делать каждый кадр
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
        // GameEventsManager.instance.input.onAttackPressed += Attack;
        GameEventsManager.instance.input.onDashPressed += Dash;
        GameEventsManager.instance.input.onReloadPressed += ReloadGun;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= AnimateChangedWeapon;
        // GameEventsManager.instance.input.onAttackPressed -= Attack;
        GameEventsManager.instance.input.onDashPressed -= Dash;
        GameEventsManager.instance.input.onReloadPressed -= ReloadGun;
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

        newData.heath = health;
        newData.nextLevelStartPosition = nextLevelStartPosition;

        newData.lightBullets = PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.LightBullets);
        newData.mediumBullets = PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.MediumBullets);
        newData.heavyBullets = PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.HeavyBullets);
        newData.shells = PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.Shells);

        newData.weapons = PlayerWeaponsManager.instance.weapons;

        PlayerDataKeeper.instance.playerData = newData;
    }

    void LoadPlayerData()
    {
        PlayerData dataToLoad = PlayerDataKeeper.instance.playerData;

        health = dataToLoad.heath;
        transform.position = dataToLoad.nextLevelStartPosition;

        PlayerWeaponsManager.instance.SetAmmoByType(AmmoTypes.LightBullets, dataToLoad.lightBullets);
        PlayerWeaponsManager.instance.SetAmmoByType(AmmoTypes.MediumBullets, dataToLoad.mediumBullets);
        PlayerWeaponsManager.instance.SetAmmoByType(AmmoTypes.HeavyBullets, dataToLoad.heavyBullets);
        PlayerWeaponsManager.instance.SetAmmoByType(AmmoTypes.Shells, dataToLoad.shells);

        PlayerWeaponsManager.instance.weapons = dataToLoad.weapons;

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

        if (PlayerWeaponsManager.instance.currentWeapon != null) bodyAnimator.SetInteger("Weapon ID", PlayerWeaponsManager.instance.currentWeapon.id);
        else bodyAnimator.SetInteger("Weapon ID", 0);

        if (move)
            legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
        else
            legsAnimator.SetFloat("Speed", 0);
    }

    private void AnimateChangedWeapon()
    {
        bodyAnimator.SetTrigger("Change Weapon");
    }

    private void RotatePlayer()
    {
        Vector2 lookDirection = InputManager.instance.lookDirection;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        rb.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void RotatePlayersLegs()
    {
        Vector2 moveDir = InputManager.instance.moveDirection;

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;

        legs.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void Attack()
    {
        if (PlayerWeaponsManager.instance.currentWeapon == null && nextAttackTime <= 0 && InputManager.instance.GetAttackPressed())
        {
            if (seriesAttack <= 0) { punchSide = 0; bodyAnimator.SetFloat("Punch Side", punchSide); }

            bodyAnimator.SetTrigger("Attack");

            var soundEffect = Instantiate(punchSound);
            soundEffect.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            Destroy(soundEffect, attackCooldown);

            Collider2D[] hitDamagedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

            foreach (var obj in hitDamagedObjects)
            {
                if (obj.gameObject == this.gameObject) continue;

                IDamagable damagedObj = obj.GetComponent<IDamagable>();

                if (damagedObj != null) damagedObj.TakeDamage(damage, transform);

            }

            if (punchSide == 0) { punchSide = 1; }
            else if (punchSide == 1) { punchSide = 0; }

            seriesAttack = seriesAttackTime;

            nextAttackTime = attackCooldown;
        }
        else if (PlayerWeaponsManager.instance.currentWeapon != null && InputManager.instance.GetAttackPressed(PlayerWeaponsManager.instance.currentWeapon.holdToAttack) && PlayerWeaponsManager.instance.currentWeaponCooldown <= 0 && !PlayerWeaponsManager.instance.currentGun.reloading)
        {
            if (PlayerWeaponsManager.instance.currentGun.ammoInMagazine > 0)
            {
                bodyAnimator.SetFloat("Attack Multiplier", PlayerWeaponsManager.instance.currentGun.firingRate); // будет ошибка при использовании оружия ближнего боя!!!
                bodyAnimator.SetTrigger("Attack");
            }

            if (CheckObstacles())
            {
                PlayerWeaponsManager.instance.currentWeapon.Attack(secondAttackPoint);
            }
            else
            {
                PlayerWeaponsManager.instance.currentWeapon.Attack(attackPoint);
            }
        }
    }

    private void RunEnable()
    {
        currentSpeed = normalSpeed * runSpeedModifier;
    }

    private void RunDisable()
    {
        currentSpeed = normalSpeed;
    }

    private void Dash()
    {
        if (canUseStamina && stamina >= maxStamina / 2)
        {
            if (InputManager.instance.moveDirection.magnitude != 0)
            {
                rb.AddForce(InputManager.instance.moveDirection * dashForce * rb.mass, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.up * dashForce * rb.mass, ForceMode2D.Impulse);
            }
            
            AudioManager.instance.PlaySoundEffect(dashSoundEffect, dashingTime);

            dashingTimeBuffer = dashingTime;
            stamina--;
        }
    }

    private bool CheckObstacles()
    {
        var hits = Physics2D.RaycastAll(rb.position, InputManager.instance.lookDirection, attackPoint.localPosition.y);

        foreach(var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.layer == 9)
                continue;
            Debug.Log("2nd atck point");
            return true;
        }
        Debug.Log("1st atck point");
        return false;
    }

    private void ReloadGun()
    {
        PlayerWeaponsManager.instance.Reload();     
    }

    public void PlayReloadingAnimation()
    {
        bodyAnimator.SetFloat("Reloading Multiplier", 1 / PlayerWeaponsManager.instance.currentGun.reloadTime);
        bodyAnimator.SetTrigger("Reload");
    }

    private void CountTimeVariables()
    {
        if (nextAttackTime > 0)
            nextAttackTime -= Time.deltaTime;

        if (seriesAttack > 0)
            seriesAttack -= Time.deltaTime;

        if (dashingTimeBuffer > 0)
            dashingTimeBuffer -= Time.deltaTime;

        if (stamina != maxStamina)
            stamina += Time.deltaTime * staminaRecoverySpeed;
    }

    private void UpdateUI()
    {
        HPStaminaManager.instance.hpSlider.value = (float)health / (float)maxHealth;
        //hpSlider.value = (float)health / (float)maxHealth;

        HPStaminaManager.instance.staminaSlider.value = stamina / maxStamina;
        //staminaSlider.value = stamina / maxDashCount;
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
