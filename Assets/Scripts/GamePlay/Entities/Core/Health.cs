using System.Collections;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// Health class, which enables things to be damaged or healed.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int _health;
        [SerializeField]
        [Tooltip("The amount of time the entity is invulnerable after being hit")]
        private float _invulnerablilityTime;
        private bool _invulnerable;
        private bool IsDead { get; set; }

        public int CurrentHealth
        {
            get => _health;
            set => _health = value;
        }

        private void Start()
        {
            CurrentHealth = _health;
        }

        public void ModifyHealth(int healthValueChange)
        {
            if (_invulnerable && healthValueChange <= 0) //if healthValueChange <=0 --> it is damage and if _invulnerable --> return and apply no damage. 
                    return;  
                
            CurrentHealth += healthValueChange;
            
            if(!_invulnerable && healthValueChange <=0)
                StartCoroutine(InvulnFrameTimer(_invulnerablilityTime));
        
            Debug.Log($"New health for {name}: {CurrentHealth}");
            
            if (CurrentHealth <= 0)
                OnDeath();
        }
        
        private void OnDeath() //TODO: Move to own script?
        {
            IsDead = true;
            gameObject.SetActive(false); //Make death-script and make event or something
        }

        private IEnumerator InvulnFrameTimer(float invulnFrameTimer)
        {
            //TODO: Temp visualization for IFrame (Color stuff)
            _invulnerable = true;
            var orgColor = GetComponent<SpriteRenderer>().color;             //Temp visualization for IFrame (Color stuff)
            GetComponent<SpriteRenderer>().color = Color.magenta;             //Temp visualization for IFrame (Color stuff)
            yield return new WaitForSeconds(invulnFrameTimer);
            GetComponent<SpriteRenderer>().color = orgColor;             //Temp visualization for IFrame (Color stuff)
            _invulnerable = false;
        }
    }
}