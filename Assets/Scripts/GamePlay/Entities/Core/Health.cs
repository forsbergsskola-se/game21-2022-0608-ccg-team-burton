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

        public void ModifyHealth(int damage)
        {
            CurrentHealth += damage;
            Debug.Log($"New health for {name}: {CurrentHealth}");
            
            if (CurrentHealth <= 0)
                OnDeath();
            
        }
        
        private void OnDeath()
        {
            IsDead = true;
            gameObject.SetActive(false); //Make death-script and make either event or something
        }

    }
}