using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace fiiFlag
{
    public partial class MainPage : ContentPage
    {
        private Image[] _selfii;    // ふぃーちゃんイメージコントロール配列
        private int Lv;             // 難易度

        // 乱数発生用変数
        System.Random r = new System.Random();

        // 最初の掛け声の配列
        string[] Startin = new string[4] {
            "Startin1","Startin2","Startin3","StartinKichiku"
        };

        // 音声再生のためのインターフェースの実装
        private IMediaPlayer VoicePlayer = DependencyService.Get<IMediaPlayer>();

        public MainPage()
        {

            // おおもとの初期化
            InitializeComponent();

            // 最初の掛け声
            VoicePlayer.PlayAsync(Startin[r.Next(0, 3)], 1.0f);

            // グリッドの準備
            Grid grid;
            grid = sg;

            // ふぃーちゃんイメージ配列の格納
            _selfii = new Image[3];
            for (int i = 0; i < 3; i++) {
                _selfii[i] = new Image();
                _selfii[i].IsVisible = false;
                _selfii[i].HorizontalOptions = LayoutOptions.FillAndExpand;
                grid.Children.Add(_selfii[i], 2, 1);
                Grid.SetRowSpan(_selfii[i], 5);
            }
            _selfii[0].Source = ImageSource.FromResource("fiiFlag.Image.Select1.png");
            _selfii[1].Source = ImageSource.FromResource("fiiFlag.Image.Select2.png");
            _selfii[2].Source = ImageSource.FromResource("fiiFlag.Image.Select3.png");

            // とりあえず難易度はふつう
            Lv = 1;
            _selfii[1].IsVisible = true;
        }

        // 簡単モード
        private void OnKantanCheckedChanged(Object o, EventArgs e)
        {
            for (int i = 0; i < 3; i++) {
                _selfii[i].IsVisible = false;
            }
            _selfii[0].IsVisible = true;
            if (Kantan.IsChecked == true) {
                Lv = 0;
                VoicePlayer.Stop();
                VoicePlayer.PlayAsync("ModeEasy", 1.0f);
            }
        }

        // 普通モード
        private void OnFutuCheckedChanged(Object o, EventArgs e)
        {
            for (int i = 0; i < 3; i++) {
                _selfii[i].IsVisible = false;
            }
            _selfii[1].IsVisible = true;
            if (Futu.IsChecked == true) {
                Lv = 1;
                VoicePlayer.Stop();
                VoicePlayer.PlayAsync("ModeNormal", 1.0f);
            }
        }

        // 鬼畜モード
        private void OnKichikuCheckedChanged(Object o, EventArgs e)
        {
            for (int i = 0; i < 3; i++) {
                _selfii[i].IsVisible = false;
            }
            _selfii[2].IsVisible = true;
            if (Kichiku.IsChecked == true) {
                Lv = 2;
                VoicePlayer.Stop();
                VoicePlayer.PlayAsync("ModeKichiku", 1.0f);
            }
        }

        // ゲーム画面へ
        private void OnGameButtonClicked(Object o, EventArgs e)
        {
            Navigation.PushModalAsync(new GamePage(Lv), false);
        }
    }
}