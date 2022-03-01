using Xamarin.Forms;

namespace fiiFlag.Controls
{
    public class BaseButton : Button
    {
        public BaseButton() : base()
        {
            this.FontAttributes = FontAttributes.Bold;
            this.FontFamily = "Fonts/JK-Gothic-M.ttf";  //ファイル名
        }
    }
}