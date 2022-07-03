namespace Entity
{
    /// <summary>
    /// Interface for everything that should be damageable, e.g. Health of enemies or player.
    /// Interface enables easy scalability if we need another health script in the future as long as it implements IDamageable.
    /// </summary>
    public interface IDamageable
    {
        public int CurrentHealth { get; set; }

        public void ModifyHealth(int healthValueChange);
    }
   
}