using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using fiiFlag.Droid.Renderer;
using System;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace fiiFlag.Droid.Renderer
{
    [Obsolete]
    public class CustomLabelRenderer : LabelRenderer
    {
        //Fontを使いまわす為
        private static Typeface _font = null;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) {
                return;
            }

            try {
                if (_font == null) {
                    //Fontが取得できていない場合は取得する
                    var fontName = e.NewElement?.FontFamily;
                    if (!string.IsNullOrEmpty(fontName)) {
                        _font = Typeface.CreateFromAsset(Forms.Context.Assets, fontName);
                    }
                }
                if (_font != null) {
                    //Fontが取得できていればフォントを設定する
                    Control.Typeface = _font;
                }
            }
            catch (Exception ex) {
                System.Console.WriteLine(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
    }
}