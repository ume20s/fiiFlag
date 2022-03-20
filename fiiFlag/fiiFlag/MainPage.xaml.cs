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
        Task PlayAsync(string title, float rate);
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
        private ExImage[] _btn;             // ボタンのイメージコントロール配列
        private Image[,,] _fii;             // ふぃーちゃんイメージコントロール配列
        private bool NowPlaying = false;    // ゲーム中フラグ
        private float Speed;                // セリフスピード
        private int Wait;                   // セリフ間ウエイト
        private int score;                  // スコア
        private int highscore;              // ハイスコア

        // ハイスコア用のファイル
        private string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "HighScore.txt";

        const int RED = 0;                  // 色定数
        const int WHITE = 1;
        const int UP = 0;                   // 旗の状態定数
        const int DOWN = 1;
        const int JITA = 0;                 // ジタバタ定数
        const int BATA = 1;
        const int PINPON = 0;               // 判定効果音
        const int BUU = 1;

        // 赤白旗の状態（赤、白の順）
        int[] Stat = new int[2] { DOWN, DOWN };

        int Asi = JITA;                     // 足の状態

        // 最初の掛け声の配列
        string[] Startin = new string[4] {
            "Startin1","Startin2","Startin3","StartinKichiku"
        };

        // カウントダウンのセリフの配列
        string[] CountDown = new string[3] {
            "ichi","ni","san"
        };

        // ゲームオーバーのセリフの配列
        string[] Gameover = new string[5] {
            "Gameover1","Gameover2","Gameover3","Gameover4","GameoverKichiku"
        };

        // 旗上げ指示セリフの構造体配列
        struct actData {
            public string comm;
            public int col;
            public int pos;

            public actData(string p1, int p2, int p3)
            {
                this.comm = p1;
                this.col = p2;
                this.pos = p3;
            }
        }
        actData[] session = {
            new actData("AkaAgeru",RED,UP),
            new actData("AkaAgenai",RED,DOWN),
            new actData("AkaSageru",RED,DOWN),
            new actData("AkaSagenai",RED,UP),
            new actData("ShiroAgeru",WHITE,UP),
            new actData("ShiroAgenai",WHITE,DOWN),
            new actData("ShiroSageru",WHITE,DOWN),
            new actData("ShiroSagenai",WHITE,UP),
            new actData("AkaAgete",RED,UP),
            new actData("AkaAgenaide",RED,DOWN),
            new actData("AkaSagete",RED,DOWN),
            new actData("AkaSagenaide",RED,UP),
            new actData("ShiroAgete",WHITE,UP),
            new actData("ShiroAgenaide",WHITE,DOWN),
            new actData("ShiroSagete",WHITE,DOWN),
            new actData("ShiroSagenaide",WHITE,UP)
        };

        // 乱数発生用変数
        System.Random r = new System.Random();

        // 音声と効果音再生のためのインターフェースの実装
        private IMediaPlayer VoicePlayer = DependencyService.Get<IMediaPlayer>();
        private ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        public MainPage()
        {
            int i,j,k;                   // 有象無象

            // 最初の掛け声
            VoicePlayer.PlayAsync(Startin[r.Next(0, 3)], 1.0f);

            // おおもとの初期化
            InitializeComponent();

            // 事前のハイスコア処理
            if (System.IO.File.Exists(localAppData)) {
                // ハイスコアファイルがあったら読み込む
                using (System.IO.StreamReader sr = new System.IO.StreamReader(localAppData)) {
                    highscore = int.Parse(sr.ReadToEnd());
                }
            } else {
                // ハイスコアファイルが無かったらハイスコアは０
                highscore = 0;
            }
            highscoreLabel.Text = " はいすこあ: " + highscore.ToString("####0");

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
            _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = true;

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
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = false;
                Stat[RED] = UP;
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = true;
            };
            _btn[1].Down += (sender, a) => {
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = false;
                Stat[WHITE] = UP;
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = true;
            };
            _btn[2].Down += (sender, a) => {
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = false;
                Stat[RED] = DOWN;
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = true;
            };
            _btn[3].Down += (sender, a) => {
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = false;
                Stat[WHITE] = DOWN;
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = true;
            };

            // タイマー処理
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () => {
                // 走ってなきゃ処理しない
                if (NowPlaying == false) {
                    return true;
                }

                // 足のジタバタ処理
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = false;
                Asi = (Asi + 1) % 2;
                _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = true;

                return true;
            });
        }

        private async void OnStartButtonClicked(Object o, EventArgs e)
        {
            int i;            // 有象無象
            int action;       // 旗上げ指示

            // ナウプレイングじゃなかったら実行
            if (NowPlaying == false) {
                // もろもろの初期化
                InitFii();

                // カウントダウン
                i = 3;
                while (i > 0) {
                    btnStart.Text = i.ToString();
                    VoicePlayer.Stop();
                    await VoicePlayer.PlayAsync(CountDown[i-1], 1.0f);
                    await Task.Run(TalkWait);
                    await System.Threading.Tasks.Task.Delay(500);
                    i -= 1;
                }

                // スタートボタンを隠して操作ボタンを表示
                btnStart.IsVisible = false;
                for (i = 0; i < 4; i++) {
                    _btn[i].IsVisible = true;
                }

                // ゲーム中
                NowPlaying = true;
                Speed = 0.6f;
                Wait = 1000;
                action = 0;

                // ゲームは25回 (+最後の1回)
                for (i = 0; i < 2; i++) {
                    action = (action + r.Next(1, 16)) % 16;
                    VoicePlayer.Stop();
                    await VoicePlayer.PlayAsync(session[action].comm, Speed);
                    await Task.Run(TalkWait);
                    await System.Threading.Tasks.Task.Delay(Wait);
                    if (Stat[session[action].col] == session[action].pos) {
                        soundEffect.SoundPlay(PINPON);
                        score += 10;
                        scoreLabel.Text = " てんすう: " + score.ToString("####0");
                        if (score > highscore) {
                            highscore = score;
                            highscoreLabel.Text = " はいすこあ: " + highscore.ToString("####0");
                        }
                    } else {
                        soundEffect.SoundPlay(BUU);
                    }
                    Speed += 0.08f;
                    Wait -= 38;
                }

                // 最後の１回は断定調のセリフで終わる
                action = r.Next(0, 8);
                VoicePlayer.Stop();
                await VoicePlayer.PlayAsync(session[action].comm, Speed);
                await Task.Run(TalkWait);
                await System.Threading.Tasks.Task.Delay(Wait);
                if (Stat[session[action].col] == session[action].pos) {
                    soundEffect.SoundPlay(PINPON);
                    score += 10;
                    scoreLabel.Text = " てんすう: " + score.ToString("####0");
                    if (score > highscore) {
                        highscore = score;
                        highscoreLabel.Text = " はいすこあ: " + highscore.ToString("####0");
                    }
                } else {
                    soundEffect.SoundPlay(BUU);
                }

                // ちょい待ってから
                await System.Threading.Tasks.Task.Delay(800);

                // ハイスコアの保存
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(localAppData)) {
                    sw.Write(highscore.ToString());
                }

                // スタートボタンを表示しなおして終了
                btnStart.Text = "げえむおおばぁ";
                btnStart.IsVisible = true;
                for (i = 0; i < 4; i++) {
                    _btn[i].IsVisible = false;
                }
                VoicePlayer.Stop();
                await VoicePlayer.PlayAsync(Gameover[r.Next(0, 4)], 1.0f);
                await Task.Run(TalkWait);
                await System.Threading.Tasks.Task.Delay(200);
                btnStart.Text = "もういっかい？";
                NowPlaying = false;
            }
        }

        private void TalkWait()
        {
            while (VoicePlayer.NowPlaying() == true) { }
            
        }

        private void InitFii()
        {
            int i, j, k;

            // 赤白旗を下げておく
            for (i = 0; i < 2; i++) {
                for (j = 0; j < 2; j++) {
                    for (k = 0; k < 2; k++) {
                        _fii[i, j, k].IsVisible = false;
                    }
                }
            }
            Stat[RED] = DOWN;
            Stat[WHITE] = DOWN;
            Asi = JITA;
            _fii[Stat[RED], Stat[WHITE], Asi].IsVisible = true;

            // スコアをゼロクリア
            score = 0;
            scoreLabel.Text = "てんすう: " + score.ToString("####0");
        }
    }
}
