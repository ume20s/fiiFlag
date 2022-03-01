using Android.Views;

namespace fiiFlag.Droid
{
    internal class MyGestureListener : GestureDetector.SimpleOnGestureListener
    {
        public ExImage ExImage { private get; set; }

        public override bool OnDown(MotionEvent e)
        {
            base.OnDown(e);
            if (ExImage != null) {
                ExImage.OnDown();
            }
            return true;
        }
    }
}