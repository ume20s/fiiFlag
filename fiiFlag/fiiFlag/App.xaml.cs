using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace fiiFlag
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // プログラムの終了
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        protected override void OnResume()
        {
        }
    }
}
