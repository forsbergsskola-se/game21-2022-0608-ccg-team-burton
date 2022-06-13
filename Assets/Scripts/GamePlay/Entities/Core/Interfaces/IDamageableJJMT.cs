
namespace Entity
{
    public interface IDamageableJJMT
    {
        public int Health { get; set; }
        public void ModifyHealth(int damage);
        
        public bool IsDead { get; set; }

        public void OnDeath();
    }
}