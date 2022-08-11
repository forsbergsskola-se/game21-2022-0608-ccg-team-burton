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
        Animator _animator;
        
        private bool IsDead { get; set; }
        
        Coins _coins;
        ItemCollector _itemCollector;

        [SerializeField] GameEvent playerDies;
        
        
        public int CurrentHealth
        {
            get => _health;
            set => _health = value;
        }

        void Awake(){
            _coins = GetComponent<Coins>();
            _itemCollector = FindObjectOfType<ItemCollector>();

            _animator = GetComponent<Animator>();

        }

        private void Start()
        {
            var equipmentHealthModifier = PlayerPrefs.GetFloat("buequipment.chest.attributevalue");
            CurrentHealth = _health+(int)equipmentHealthModifier;
        }

        public void ModifyHealth(int healthValueChange)
        {
            if (_invulnerable && healthValueChange <= 0) //if healthValueChange <=0 --> it is damage and if _invulnerable --> return and apply no damage. 
                    return;  
                
            CurrentHealth += healthValueChange;

            //if statement for vibrate toggle bool = true vibrate
            Handheld.Vibrate();
            OnHealthChanged?.Invoke(CurrentHealth);


            if (!_invulnerable && healthValueChange <= 0)
            {
                StartCoroutine(InvulnFrameTimer(_invulnerablilityTime));

                if (CurrentHealth > 0){
                    _animator.SetTrigger(Animator.StringToHash("TakeDmg"));
                }
                
            }
        
            Debug.Log($"New health for {name}: {CurrentHealth}");

            if (CurrentHealth <= 0)
            {
                Debug.Log("Animtrigger");
                _animator.SetTrigger(Animator.StringToHash("Dead"));
                StartCoroutine(StartDeath());
            }
        }

        private IEnumerator StartDeath()
        {
            yield return new WaitForSeconds(1.5f);
            OnDeath();
        }
        
        private void OnDeath() //TODO: Move to own script?
        {
            //Make death-script and make event or something
            

            if (gameObject.CompareTag("Enemy")){
                _itemCollector._coinCounter += _coins._coinValue;
                _itemCollector.UpdateCoinText(_itemCollector._coinCounter);
            }

            if (gameObject.CompareTag("Player")){
                _itemCollector._coinCounter -= _itemCollector._coinCounter;
                _itemCollector.UpdateCoinText(_itemCollector._coinCounter);
            }
            gameObject.SetActive(false);
            
            playerDies.Invoke();

            IsDead = true;

        }

        private IEnumerator InvulnFrameTimer(float invulnFrameTimer)
        {
            //TODO: Temp visualization for IFrame (Color stuff)
            _invulnerable = true;
            var originalColor = GetComponent<SpriteRenderer>().color;             //Temp visualization for IFrame (Color stuff)
            GetComponent<SpriteRenderer>().color = Color.red;             //Temp visualization for IFrame (Color stuff)
            yield return new WaitForSeconds(invulnFrameTimer);
            GetComponent<SpriteRenderer>().color = originalColor;             //Temp visualization for IFrame (Color stuff)
            _invulnerable = false;
        }
    }
}