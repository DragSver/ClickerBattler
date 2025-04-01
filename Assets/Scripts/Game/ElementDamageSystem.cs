using Kolobrod.Game.Enemy;

namespace Game
{
    public static class ElementDamageSystem
    {
        public static float CalculateDamage(Elements attackElement, Elements enemyElement, float damage)
        {
            if ((attackElement == Elements.Sun && enemyElement == Elements.Moon)
                || (attackElement == Elements.Moon && enemyElement == Elements.Blood)
                || (attackElement == Elements.Blood && enemyElement == Elements.Water) 
                || (attackElement == Elements.Water && enemyElement == Elements.Sun)) 
                return damage * 1.5f;
            
            if ((enemyElement == Elements.Sun && attackElement == Elements.Moon)
                     || (enemyElement == Elements.Moon && attackElement == Elements.Blood)
                     || (enemyElement == Elements.Blood && attackElement == Elements.Water)
                     || (enemyElement == Elements.Water && attackElement == Elements.Sun))
                return damage * 0.5f;

            return damage;
        }
    }
}