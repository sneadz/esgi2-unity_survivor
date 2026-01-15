# Documentation - Système Ennemis

**Développé par : Maxime**  
**Branche Git : maxime/enemies-gameplay**  
**Statut : Terminé et testé**

---

## Ce qui a été fait

### Scripts

Tous les scripts sont dans `Assets/Scripts/Enemies/`

**EnemyData.cs**
- ScriptableObject qui stocke les stats d'un ennemi (vie, vitesse, dégâts, etc.)
- Permet de créer facilement des variantes d'ennemis
- Un exemple est déjà créé : "Zombie Basique.asset"

**EnemyHealth.cs**
- Gère la vie de l'ennemi
- Fonction publique `TakeDamage(float damage)` que les projectiles peuvent appeler
- Flash rouge quand l'ennemi est touché
- Détruit l'ennemi quand sa vie tombe à 0

**EnemyController.cs**
- Fait se déplacer l'ennemi vers le joueur automatiquement
- L'ennemi doit trouver un GameObject avec le tag "Player"
- Fait tourner l'ennemi vers le joueur
- Inflige des dégâts au contact avec un système de cooldown
- Utilise l'interface IPlayerHealth pour communiquer avec le joueur

**IPlayerHealth.cs**
- Interface simple que le script Player doit implémenter
- Définit juste une fonction : `void TakeDamage(float damage)`
- Permet aux ennemis et au joueur de communiquer sans dépendance directe

### Assets créés

**Prefab : Enemy_Basic**
- Localisation : `Assets/Prefabs/Enemies/Enemy_Basic.prefab`
- Cube rouge avec tous les composants nécessaires
- Rigidbody configuré (gravité activée, rotation bloquée)
- Tag "Enemy" déjà assigné
- Scripts EnemyHealth et EnemyController attachés
- Prêt à être spawné par Sofiane

**ScriptableObject : Zombie Basique**
- Localisation : `Assets/Scripts/Enemies/Zombie Basique.asset`
- Stats par défaut :
    - Vie : 10 HP
    - Vitesse : 2 unités/sec
    - Dégâts : 5 HP par attaque
    - Cooldown : 1 seconde entre les attaques
- Ces valeurs peuvent être modifiées dans l'Inspector Unity

**Scène de test : Scene_maxime**
- Localisation : `Assets/Scenes/Scene_maxime.unity`
- Contient un sol, un ennemi et un cube de test avec tag "Player"
- Sert à tester le comportement des ennemis

---

## Ce qui est nécessaire pour l'intégration

### Jules-Edouard (Joueur)

Tu dois faire deux modifications dans ton code :

**1. Implémenter l'interface IPlayerHealth**
**2. Modifier le script Projectile**

Assure-toi aussi que ton GameObject joueur a bien le tag "Player" dans l'Inspector Unity.

### Sofiane (Spawn)

Le prefab Enemy_Basic est prêt à être utilisé.

**Emplacement :** `Assets/Prefabs/Enemies/Enemy_Basic.prefab`

**Comment l'utiliser :**
```csharp
// Dans ton script de spawn
public GameObject enemyPrefab; // Glisse-dépose Enemy_Basic ici dans l'Inspector

void SpawnEnemy(Vector3 position)
{
    Instantiate(enemyPrefab, position, Quaternion.identity);
}
```

L'ennemi va automatiquement :
- Chercher le joueur (tag "Player")
- Se déplacer vers lui
- L'attaquer au contact

**Ajustement de difficulté :**
Si tu veux modifier les stats des ennemis (plus rapides, plus de vie, etc.), tu peux :
- Soit créer un nouveau ScriptableObject depuis le menu "Create > Survivor > Enemy Data"
- Soit modifier directement "Zombie Basique.asset" dans l'Inspector

### Yanis (XP)

Dans le script EnemyHealth.cs, j'ai laissé un TODO pour toi dans la fonction `Die()` :
```csharp
private void Die()
{
    // TODO : Yanis - Spawn une orbe d'XP ici
    Destroy(gameObject);
}
```

Tu peux ajouter ton code de spawn d'XP à cet endroit. La fonction est appelée automatiquement quand l'ennemi meurt.

### Lily (UI)

Rien de spécifique à faire de ton côté pour le moment. Le système d'ennemis est autonome.
Si tu veux afficher la vie des ennemis à l'écran plus tard, tu peux accéder à `currentHealth` dans EnemyHealth.

---

## Tests effectués

Les tests suivants ont été validés dans ma scène de test :

- L'ennemi se déplace vers le joueur : OK
- L'ennemi inflige des dégâts au contact : OK (vérifié avec des logs dans la Console)
- L'ennemi meurt quand sa vie tombe à 0 : OK
- Le prefab peut être instancié plusieurs fois : OK
- Pas d'erreurs dans la Console Unity : OK

---

## Notes techniques

**Configuration du Rigidbody (déjà faite sur le prefab) :**
- Use Gravity : activé
- Freeze Rotation X, Y, Z : activés (empêche l'ennemi de tomber sur le côté)

**Tags requis :**
- Le joueur doit avoir le tag "Player"
- Les ennemis ont le tag "Enemy"

**Collisions :**
- L'ennemi utilise OnCollisionStay (collision 3D continue)
- Les projectiles doivent utiliser des Triggers (OnTriggerEnter)

---

## En cas de problème

**L'ennemi ne bouge pas :**
- Vérifie que le joueur a bien le tag "Player"
- Vérifie que le Rigidbody n'est pas en mode Kinematic

**L'ennemi n'inflige pas de dégâts :**
- Vérifie que le joueur implémente bien l'interface IPlayerHealth
- Vérifie les logs dans la Console pour voir les erreurs

**Les projectiles ne tuent pas l'ennemi :**
- Vérifie que le projectile appelle bien `TakeDamage()` dans OnTriggerEnter
- Vérifie que l'ennemi a bien le tag "Enemy"

Si vous avez des questions ou si quelque chose ne fonctionne pas, contactez-moi sur Discord.