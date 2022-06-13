using System;
using UnityEngine;

namespace Entity
{
    public class HealthJjmt : MonoBehaviour, IDamageableJJMT
    {

        [SerializeField]
        private int _health;
        public int Health { get; set; }
        public bool IsDead { get; set; }
        public void OnDeath()
        {
            gameObject.SetActive(false);
        }


        private void Start()
        {
            Health = _health;
        }

        public void ModifyHealth(int damage)
        {
            Health += damage;
            Debug.Log($"New health for {name}: {Health}");
            if (Health <= 0)
            {
                IsDead = true;
                //Todo: Call destroyer
                OnDeath();
            }
        }

    }
}