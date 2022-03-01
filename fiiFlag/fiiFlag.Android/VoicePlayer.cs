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

        [Obsolete]
        private async Task StartPlayerAsync(string title)
        {
            var resourceId = (int)typeof(Resource.Raw).GetField(title).GetValue(null);

            await Task.Run(() => {
                if (player == null) {
                    player = new MediaPlayer();
                    player.SetAudioStreamType(Stream.Music);
                    player = MediaPlayer.Create(global::Android.App.Application.Context, resourceId);
                    player.Looping = false;
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
        public async Task PlayAsync(string title)
        {
            await StartPlayerAsync(title);
        }

        public void Stop()
        {
            StopPlayer();
        }
    }
}