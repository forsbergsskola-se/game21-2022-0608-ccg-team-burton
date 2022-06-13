
namespace Entity
{
    /// <summary>
    /// Interface for everything that should be damageable, e.g. Health of enemies or player. Interface enables easy scalability if we need another health script in the future as long as it implements the interface.
    /// This in turn does so that no matter what kind oif health script is attached, health-modification will be applied
    /// </summary>
    public interface IDamageable
    {
        public int CurrentHealth { get; set; }

        public void ModifyHealth(int damage);
    }
   
}