using Game.Enemy;

namespace Game
{
    public static class ElementDamageSystem
    {
        public static float CalculateDamage(Elements attackElement, Elements enemyElement, float damage)
        {
            var influence = GetElementsInfluence(attackElement, enemyElement);
            switch (influence)
            {
                case ElementsInfluence.Strong:
                    return damage * 1.5f;
                case ElementsInfluence.Weakly:
                    return damage * 0.5f;
                case ElementsInfluence.Standard:
                default:
                    return damage;
            }
        }

        public static ElementsInfluence GetElementsInfluence(Elements attackElement, Elements enemyElement)
        {
            if ((attackElement == Elements.Sun && enemyElement == Elements.Moon)
                || (attackElement == Elements.Moon && enemyElement == Elements.Blood)
                || (attackElement == Elements.Blood && enemyElement == Elements.Water)
                || (attackElement == Elements.Water && enemyElement == Elements.Sun))
                return ElementsInfluence.Strong;

            if ((enemyElement == Elements.Sun && attackElement == Elements.Moon)
                || (enemyElement == Elements.Moon && attackElement == Elements.Blood)
                || (enemyElement == Elements.Blood && attackElement == Elements.Water)
                || (enemyElement == Elements.Water && attackElement == Elements.Sun))
                return ElementsInfluence.Weakly;

            return ElementsInfluence.Standard;
        } 
    }
}