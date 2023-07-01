namespace Features.Bots
{
    public interface IMortal
    {
        void Damage(int damageValue, bool byPlayer);
        void FullDamage();
    }
}