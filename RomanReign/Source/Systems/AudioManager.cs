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
            set { m_instance.Volume = value; }
        }

        public float FadeIn;
        public float FadeOut;

        float m_timeSinceStarted;
        float m_timeSinceStopped;

        float m_startVolume;
        float m_stopVolume;

        bool m_starting;
        bool m_stopping;

        public LoopingMusic(SoundEffect soundEffect)
        {
            m_instance = soundEffect.CreateInstance();
            m_instance.IsLooped = true;
        }

        public void Update(GameTime gameTime)
        {
            m_timeSinceStarted += (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_timeSinceStopped += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (m_starting)
            {
                if (m_instance.State != SoundState.Playing)
                    m_instance.Play();

                if (m_timeSinceStarted < FadeIn)
                {
                    float vol = MathHelper.Lerp(m_startVolume, 1, m_timeSinceStarted / FadeIn);
                    m_instance.Volume = vol;
                }
                else
                {
                    m_starting = false;
                }
            }

            if (m_stopping)
            {
                if (m_timeSinceStopped < FadeOut)
                {
                    float vol = MathHelper.Lerp(m_stopVolume, 0, m_timeSinceStopped / FadeOut);
                    m_instance.Volume = vol;
                }
                else
                {
                    m_instance.Stop();
                    m_stopping = false;
                }
            }
        }

        public void Play()
        {
            m_starting = true;
            m_timeSinceStarted = 0;
            m_startVolume = Volume;
        }

        public void Stop()
        {
            m_stopping = true;
            m_timeSinceStopped = 0;
            m_stopVolume = Volume;
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
