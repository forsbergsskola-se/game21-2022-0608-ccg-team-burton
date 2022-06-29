using System;
using System.Collections;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// Health class, which enables things to be damaged or healed.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        
                
        public Action<int> OnHealthChanged;
        [SerializeField]
        private int _health;
        
        [SerializeField]
        [Tooltip("The amount of time the entity is invulnerable after being hit")]
        private float _invulnerablilityTime;
        private bool _invulnerable;
        public FMODUnity.EventReference TakeDamageSoundFile;
        private FMOD.Studio.EventInstance _takeDamageSound;
        public FMODUnity.EventReference DeathSoundFile;
        private FMOD.Studio.EventInstance _deathSound;
        Animator _animator;
        
        private bool IsDead { get; set; }
        
        Coins _coins;
        ItemCollector _itemCollector;
        
        private SoundMananger _soundMananger;
        
        
        
        public int CurrentHealth
        {
            get => _health;
            set => _health = value;
        }

        void Awake(){
            _coins = GetComponent<Coins>();
            _itemCollector = FindObjectOfType<ItemCollector>();

            _soundMananger = FindObjectOfType<SoundMananger>();

            _animator = GetComponent<Animator>();

        }

        private void Start()
        {
            CurrentHealth = _health;
            _takeDamageSound = FMODUnity.RuntimeManager.CreateInstance(TakeDamageSoundFile);
            _deathSound = FMODUnity.RuntimeManager.CreateInstance(DeathSoundFile);

        }

        public void ModifyHealth(int healthValueChange)
        {
            if (_invulnerable && healthValueChange <= 0) //if healthValueChange <=0 --> it is damage and if _invulnerable --> return and apply no damage. 
                    return;  
                
            CurrentHealth += healthValueChange;
            OnHealthChanged?.Invoke(CurrentHealth);


            if (!_invulnerable && healthValueChange <= 0)
            {
                StartCoroutine(InvulnFrameTimer(_invulnerablilityTime));
                _soundMananger.PlaySound(_takeDamageSound);
                
            }
        
            Debug.Log($"New health for {name}: {CurrentHealth}");
            
            if (CurrentHealth <= 0)
                OnDeath();
        }
        
        private void OnDeath() //TODO: Move to own script?
        {
            _animator.SetTrigger("Dead");
            _soundMananger.PlaySound(_deathSound);
            IsDead = true;
            //Make death-script and make event or something
            

            if (gameObject.CompareTag("Enemy")){
                _itemCollector._coinCounter += _coins._coinValue;
                _itemCollector.UpdateCoinText(_itemCollector._coinCounter);
            }

            if (gameObject.CompareTag("Player")){
                _itemCollector._coinCounter -= _itemCollector._coinCounter;
                _itemCollector.UpdateCoinText(_itemCollector._coinCounter);
            }
            
        }

        private IEnumerator InvulnFrameTimer(float invulnFrameTimer)
        {
            //TODO: Temp visualization for IFrame (Color stuff)
            _invulnerable = true;
            var originalColor = GetComponent<SpriteRenderer>().color;             //Temp visualization for IFrame (Color stuff)
            GetComponent<SpriteRenderer>().color = Color.magenta;             //Temp visualization for IFrame (Color stuff)
            yield return new WaitForSeconds(invulnFrameTimer);
            GetComponent<SpriteRenderer>().color = originalColor;             //Temp visualization for IFrame (Color stuff)
            _invulnerable = false;
        }
    }
}