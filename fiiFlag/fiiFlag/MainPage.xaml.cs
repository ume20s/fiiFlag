using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace fiiFlag
{
    // 音声再生のためのインターフェースの作成
    public interface IMediaPlayer
    {
        Task PlayAsync(string title);
        void Stop();
        bool NowPlaying();
    }

    // 効果音再生のためのインターフェースの作成
    public interface ISoundEffect
    {
        void SoundPlay(int c);
    }

    public partial class MainPage : ContentPage
    {
        private ExImage[] _btn;         // ボタンのイメージコントロール配列
        private Image[,,] _fii;         // ふぃーちゃんイメージコントロール配列
        bool NowPlaying = false;        // ゲーム中フラグ
        const int RED = 0;              // 色定数
        const int WHITE = 1;
        const int UP = 0;               // 旗の状態定数
        const int DOWN = 1;
        const int JITA = 0;             // ジタバタ定数
        const int BATA = 1;

        int RedFlag = DOWN;             // 赤白旗の状態
        int WhiteFlag = DOWN;
        int Asi = JITA;                 // 足の状態

        // 音声と効果音再生のためのインターフェースの実装
        private IMediaPlayer VoicePlayer = DependencyService.Get<IMediaPlayer>();
        private ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        public MainPage()
        {
            int i,j,k;                   // 有象無象

            // おおもとの初期化
            InitializeComponent();

            // グリッドの準備
            Grid grid;
            grid = g;

            // ふぃーちゃんイメージ配列の格納
            _fii = new Image[2,2,2];    // _fii[赤,白,ジタバタ]
            for (i = 0; i < 2; i++) {
                for(j=0;j<2; j++) {
                    for(k=0;k<2; k++) {
                        _fii[i,j,k] = new Image();
                        _fii[i, j, k].IsVisible = false;
                        grid.Children.Add(_fii[i, j, k], 0, 1);
                        Grid.SetColumnSpan(_fii[i, j, k], 2);
                    }
                }
            }
            _fii[UP, UP, JITA].Source = ImageSource.FromResource("fiiFlag.Image.RuWu_1.png");
            _fii[UP, UP, BATA].Source = ImageSource.FromResource("fiiFlag.Image.RuWu_2.png");
            _fii[UP, DOWN, JITA].Source = ImageSource.FromResource("fiiFlag.Image.RuWd_1.png");
            _fii[UP, DOWN, BATA].Source = ImageSource.FromResource("fiiFlag.Image.RuWd_2.png");
            _fii[DOWN, UP, JITA].Source = ImageSource.FromResource("fiiFlag.Image.RdWu_1.png");
            _fii[DOWN, UP, BATA].Source = ImageSource.FromResource("fiiFlag.Image.RdWu_2.png");
            _fii[DOWN, DOWN, JITA].Source = ImageSource.FromResource("fiiFlag.Image.RdWd_1.png");
            _fii[DOWN, DOWN, BATA].Source = ImageSource.FromResource("fiiFlag.Image.RdWd_2.png");

            // とりあえず赤白とも下げておく
            _fii[RedFlag, WhiteFlag, Asi].IsVisible = true;

            // 操作ボタンイメージ配列の格納
            _btn = new ExImage[4];
            for (i = 0; i < 4; i++) {
                _btn[i] = new ExImage();
                _btn[i].Margin = 3;
                _btn[i].IsVisible = false;
                grid.Children.Add(_btn[i], i % 2, i / 2 + 2);
            }
            _btn[0].Source = ImageSource.FromResource("fiiFlag.Image.btnRu.png");
            _btn[1].Source = ImageSource.FromResource("fiiFlag.Image.btnWu.png");
            _btn[2].Source = ImageSource.FromResource("fiiFlag.Image.btnRd.png");
            _btn[3].Source = ImageSource.FromResource("fiiFlag.Image.btnWd.png");

            // タッチイベントの実装
            _btn[0].Down += (sender, a) => {
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = false;
                RedFlag = UP;
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = true;
                soundEffect.SoundPlay(0);
            };
            _btn[1].Down += (sender, a) => {
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = false;
                WhiteFlag = UP;
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = true;
            };
            _btn[2].Down += (sender, a) => {
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = false;
                RedFlag = DOWN;
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = true;
                VoicePlayer.PlayAsync("buu");
            };
            _btn[3].Down += (sender, a) => {
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = false;
                WhiteFlag = DOWN;
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = true;
            };

            // タイマー処理
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () => {
                // 走ってなきゃ処理しない
                if (NowPlaying == false) {
                    return true;
                }

                // 足のジタバタ処理
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = false;
                Asi = (Asi + 1) % 2;
                _fii[RedFlag, WhiteFlag, Asi].IsVisible = true;

                return true;
            });
        }

        public async void OnStartButtonClicked(Object o, EventArgs e)
        {
            int i;

            // カウントダウン
            i = 5;
            while (true && i > 0) {
                btnStart.Text = i.ToString();
                await System.Threading.Tasks.Task.Delay(1000);
                i -= 1;
            }
            // スタートボタンを隠して操作ボタンを表示
            btnStart.IsVisible = false;
            for (i=0; i<4; i++) {
                _btn[i].IsVisible = true;
            }

            // ゲーム中
            NowPlaying = true;
        }
    }
}
