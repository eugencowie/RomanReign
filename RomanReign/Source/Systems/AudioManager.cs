using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace RomanReign
{
    class LoopingMusic
    {
        SoundEffectInstance m_instance;

        public float Volume
        {
            get { return m_instance.Volume; }
            set { m_instance.Volume = value; TargetVolume = value; }
        }

        public float Pitch
        {
            get { return m_instance.Pitch; }
            set { m_instance.Pitch = value; TargetPitch = value; }
        }

        public float TargetVolume;
        public float TargetPitch;

        public delegate void OnLoopDelegate();
        public event OnLoopDelegate OnLoop;

        public LoopingMusic(SoundEffect soundEffect)
        {
            m_instance = soundEffect.CreateInstance();

            TargetVolume = 1f;
        }

        public void Update(GameTime gameTime)
        {
            if (m_instance.State == SoundState.Stopped)
            {
                m_instance.Play();

                OnLoop?.Invoke();
            }

            if (m_instance.Volume != TargetVolume)
            {
                m_instance.Volume = MathHelper.Lerp(m_instance.Volume, TargetVolume, (float)gameTime.ElapsedGameTime.TotalSeconds/2);
            }

            if (m_instance.Pitch != TargetPitch)
            {
                m_instance.Pitch = MathHelper.Lerp(m_instance.Pitch, TargetPitch, (float)gameTime.ElapsedGameTime.TotalSeconds/2);
            }
        }
    }

    class AudioManager
    {
        public LoopingMusic BackgroundMusic;

        public void Update(GameTime gameTime)
        {
            BackgroundMusic?.Update(gameTime);
        }
    }
}
