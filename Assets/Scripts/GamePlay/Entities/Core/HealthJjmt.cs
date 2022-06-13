using System;
using UnityEngine;

namespace Entity
{
    public class HealthJjmt : MonoBehaviour, IDamageableJJMT
    {

        [SerializeField]
        private int _health;
        public int Health { get; set; }

        private void Start()
        {
            Health = _health;
        }

        public void ModifyHealth(int damage)
        {
            Health += damage;
            Debug.Log($"New health: {Health}");    
        }
    }
}