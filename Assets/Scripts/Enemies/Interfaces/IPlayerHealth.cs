// Permet aux ennemis d'infliger des dégâts sans dépendre du code exact du joueur (à utiliser par Jules-Edouard)
public interface IPlayerHealth
{
    void TakeDamage(float damage);
}