using System;

namespace Feature.Interface.Presenter
{
    public interface IEnemyPresenter
    {
        public event Action OnDead;
        public void Spawned();

        public void Death();

        public void Pause();

        public void Resume();

        // -- health
        public void TakeDamage(int damage);
    }
}