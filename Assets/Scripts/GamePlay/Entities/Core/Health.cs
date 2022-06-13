using UnityEngine;

namespace Entity
{
    public class Health : MonoBehaviour
    {

        [SerializeField]
        private int _health;
        private int currentHealth { get; set; }
        private bool IsDead { get; set; }

        private void OnDeath()
        {
            IsDead = true;
            gameObject.SetActive(false); // disable player here
        }

        private void Start()
        {
            currentHealth = _health;
        }

        public void ModifyHealth(int damage)
        {
            currentHealth += damage;
            Debug.Log($"New health for {name}: {currentHealth}");
            if (currentHealth <= 0)
            {
                OnDeath();
            }
        }
    }
}