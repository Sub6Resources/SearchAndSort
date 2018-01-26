using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SearchAndSort
{
    public class Sound
    {
        private SoundEffect death2Sound;
        private SoundEffect derpSound;
        private SoundEffect derp2Sound;
        private SoundEffect derpDeathSound;
        private SoundEffect derpHurtSound;
        private SoundEffect elite1Sound;
        private SoundEffect eliteAngerSound;
        private SoundEffect eliteDeathSound;
        private SoundEffect eliteHitSound;
        private SoundEffect eliteRoarSound;
        private SoundEffect explosion;
        private SoundEffect kamiAngerSound;
        private SoundEffect kamiChargeSound;
        private SoundEffect kamiDeathSound;
        private SoundEffect kamiHurtSound;
        private SoundEffect laserShootSound;
        public enum Sounds { DEATH2, DERP, DERP2, DERPDEATH, DERPHURT, ELITE1, ELITEANGER, ELITEDEATH, ELITEHIT, ELITEROAR, EXPLOSION, KAMIANGER, KAMICHARGE, KAMIDEATH, KAMIHURT, LASERSHOOT }
        public Sound(Game1 game) {
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
            switch(sound)
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
