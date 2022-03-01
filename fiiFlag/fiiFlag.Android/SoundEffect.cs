using Android.Media;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(fiiFlag.Droid.SoundEffect))]
namespace fiiFlag.Droid
{
    class SoundEffect : ISoundEffect
    {
        SoundPool soundPool;
        int[] soundPoolId = new int[2];

        // もろもろの初期化とファイルの読み込み
        public SoundEffect()
        {
            int SOUND_POOL_MAX = 2;

            AudioAttributes attr = new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Media)
                .SetContentType(AudioContentType.Music)
                .Build();
            soundPool = new SoundPool.Builder()
               .SetAudioAttributes(attr)
               .SetMaxStreams(SOUND_POOL_MAX)
               .Build();
            soundPoolId[0] = soundPool.Load(Android.App.Application.Context, Resource.Raw.pinpon, 1);
            soundPoolId[1] = soundPool.Load(Android.App.Application.Context, Resource.Raw.buu, 1);
        }

        // 効果音の再生
        public void SoundPlay(int c)
        {
            soundPool.Play(soundPoolId[c], 1.0F, 1.0F, 0, 0, 1.0F);
        }
    }
}