using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Media;

[assembly: Dependency(typeof(fiiFlag.Droid.VoicePlayer))]
namespace fiiFlag.Droid
{
    class VoicePlayer : IMediaPlayer
    {
        MediaPlayer player = null;
        PlaybackParams playbackParams;

        [Obsolete]
        private async Task StartPlayerAsync(string title, float rate)
        {
            var resourceId = (int)typeof(Resource.Raw).GetField(title).GetValue(null);

            await Task.Run(() => {
                if (player == null) {
                    // MediaPlayer関係の初期化
                    player = new MediaPlayer();
                    player.SetAudioStreamType(Stream.Music);
                    player = MediaPlayer.Create(global::Android.App.Application.Context, resourceId);
                    player.Looping = false;

                    // PlaybackParams関係の初期化
                    playbackParams = new PlaybackParams();
                    playbackParams.SetSpeed(rate);
                    player.PlaybackParams = playbackParams;

                    // 再生開始
                    player.Start();
                } else {
                    if (player.IsPlaying == true) {
                        player.Pause();
                    } else {
                        player.Start();
                    }
                }
            });
        }

        // 現在再生中かどうか
        public bool NowPlaying()
        {
            if ((player != null)) {
                if (player.IsPlaying == true) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        private void StopPlayer()
        {
            if ((player != null)) {
                if (player.IsPlaying) {
                    player.Stop();
                }
                player.Release();
                player = null;
            }
        }

        public void Restart()
        {
            if ((player != null)) {
                player.Start();
            }
        }

        public void Pause()
        {
            if ((player != null)) {
                if (player.IsPlaying) {
                    player.Pause();
                }
            }
        }

        [Obsolete]
        public async Task PlayAsync(string title, float rate)
        {
            await StartPlayerAsync(title, rate);
        }

        public void Stop()
        {
            StopPlayer();
        }
    }
}