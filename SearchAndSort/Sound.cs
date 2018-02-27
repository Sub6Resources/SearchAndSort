using Microsoft.Xna.Framework.Audio;

namespace SearchAndSort
{
    public class Sound
    {
        public enum Sounds
        {
            DEATH2,
            DERP,
            DERP2,
            DERPDEATH,
            DERPHURT,
            ELITE1,
            ELITEANGER,
            ELITEDEATH,
            ELITEHIT,
            ELITEROAR,
            EXPLOSION,
            KAMIANGER,
            KAMICHARGE,
            KAMIDEATH,
            KAMIHURT,
            LASERSHOOT
        }

        private readonly SoundEffect death2Sound;
        private readonly SoundEffect derp2Sound;
        private readonly SoundEffect derpDeathSound;
        private readonly SoundEffect derpHurtSound;
        private readonly SoundEffect derpSound;
        private readonly SoundEffect elite1Sound;
        private readonly SoundEffect eliteAngerSound;
        private readonly SoundEffect eliteDeathSound;
        private SoundEffect eliteHitSound;
        private readonly SoundEffect eliteRoarSound;
        private readonly SoundEffect explosion;
        private readonly SoundEffect kamiAngerSound;
        private readonly SoundEffect kamiChargeSound;
        private readonly SoundEffect kamiDeathSound;
        private readonly SoundEffect kamiHurtSound;
        private readonly SoundEffect laserShootSound;

        public Sound(Game1 game)
        {
            death2Sound = game.Content.Load<SoundEffect>("Death2");
            derpSound = game.Content.Load<SoundEffect>("Derp");
            derp2Sound = game.Content.Load<SoundEffect>("Derp2");
            derpDeathSound = game.Content.Load<SoundEffect>("eenewDerpDeath");
            derpHurtSound = game.Content.Load<SoundEffect>("DerpHurt");
            elite1Sound = game.Content.Load<SoundEffect>("Elite1");
            eliteAngerSound = game.Content.Load<SoundEffect>("EliteAnger");
            eliteDeathSound = game.Content.Load<SoundEffect>("EliteDeath");
            eliteRoarSound = game.Content.Load<SoundEffect>("EliteRoar");
            explosion = game.Content.Load<SoundEffect>("Explosion");
            kamiAngerSound = game.Content.Load<SoundEffect>("KamiAnger");
            kamiChargeSound = game.Content.Load<SoundEffect>("KamiCharge");
            kamiDeathSound = game.Content.Load<SoundEffect>("eenewKamiDeath");
            kamiHurtSound = game.Content.Load<SoundEffect>("KamiHurt");
            laserShootSound = game.Content.Load<SoundEffect>("Laser_Shoot");
        }

        public void PlaySound(Sounds sound)
        {
            switch (sound)
            {
                case Sounds.DEATH2:
                    death2Sound.Play();
                    break;
                case Sounds.DERP:
                    derpSound.Play();
                    break;
                case Sounds.DERP2:
                    derp2Sound.Play();
                    break;
                case Sounds.DERPDEATH:
                    derpDeathSound.Play();
                    break;
                case Sounds.DERPHURT:
                    derpHurtSound.Play();
                    break;
                case Sounds.ELITE1:
                    elite1Sound.Play();
                    break;
                case Sounds.ELITEANGER:
                    eliteAngerSound.Play();
                    break;
                case Sounds.ELITEDEATH:
                    eliteDeathSound.Play();
                    break;
                case Sounds.ELITEHIT:
                    eliteHitSound.Play();
                    break;
                case Sounds.ELITEROAR:
                    eliteRoarSound.Play();
                    break;
                case Sounds.EXPLOSION:
                    explosion.Play();
                    break;
                case Sounds.KAMIANGER:
                    kamiAngerSound.Play();
                    break;
                case Sounds.KAMICHARGE:
                    kamiChargeSound.Play();
                    break;
                case Sounds.KAMIDEATH:
                    kamiDeathSound.Play();
                    break;
                case Sounds.KAMIHURT:
                    kamiHurtSound.Play();
                    break;
                case Sounds.LASERSHOOT:
                    laserShootSound.Play();
                    break;
            }
        }
    }
}