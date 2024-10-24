using ProjectLightsOut.DevUtils;
using ProjectLightsOut.Hittable;

namespace ProjectLightsOut.Managers
{
    public class OnEnemyRegister : GameEvent
    {
        public Enemy Enemy;

        public OnEnemyRegister(Enemy enemy)
        {
            Enemy = enemy;
        }
    }

    public class OnEnemyDead : GameEvent
    {
        public Enemy Enemy;

        public OnEnemyDead(Enemy enemy)
        {
            Enemy = enemy;
        }
    }

    public class OnProjectileShoot : GameEvent
    {}

    public class OnProjectileDestroy : GameEvent
    {}

    public class OnLevelComplete : GameEvent
    {}
}
