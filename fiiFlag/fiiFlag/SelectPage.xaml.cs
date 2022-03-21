using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace fiiFlag
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPage : ContentPage
    {
        private Image[] _selfii;        // ふぃーちゃんイメージコントロール配列

        // 音声と効果音再生のためのインターフェースの実装
        private IMediaPlayer VoicePlayer = DependencyService.Get<IMediaPlayer>();

        public SelectPage()
        {
            int i;                      // 有象無象

            // おおもとの初期化
            InitializeComponent();

            // グリッドの準備
            Grid grid;
            grid = sg;

            // ふぃーちゃんイメージ配列の格納
            _selfii = new Image[3];
            for (i = 0; i < 3; i++) {
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
            _selfii[1].IsVisible = true;
        }

        private void OnKantanCheckedChanged(Object o, EventArgs e)
        {
            int i;

            for (i = 0; i < 3; i++) {
                _selfii[i].IsVisible = false;
            }
            _selfii[0].IsVisible = true;
            if(Kantan.IsChecked == true) {
                VoicePlayer.Stop();
                VoicePlayer.PlayAsync("ModeEasy", 1.0f);
            }
        }
        private void OnFutuCheckedChanged(Object o, EventArgs e)
        {
            int i;

            for (i = 0; i < 3; i++) {
                _selfii[i].IsVisible = false;
            }
            _selfii[1].IsVisible = true;
            if (Futu.IsChecked == true) {
                VoicePlayer.Stop();
                VoicePlayer.PlayAsync("ModeNormal", 1.0f);
            }
        }
        private void OnKichikuCheckedChanged(Object o, EventArgs e)
        {
            int i;

            for (i = 0; i < 3; i++) {
                _selfii[i].IsVisible = false;
            }
            _selfii[2].IsVisible = true;
            if (Kichiku.IsChecked == true) {
                VoicePlayer.Stop();
                VoicePlayer.PlayAsync("ModeKichiku", 1.0f);
            }
        }


        private void OnGameButtonClicked(Object o, EventArgs e)
        {
            Navigation.PopModalAsync(false);
        }
    }
}