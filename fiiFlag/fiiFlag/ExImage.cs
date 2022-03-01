using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace fiiFlag
{
    // 押下イベント取得のためにImageクラスを拡張
    public class ExImage : Image
    {
        public event EventHandler Down;
        public bool OnDown()
        {
            if (Down != null) {
                Down(this, new EventArgs());
            }
            return true;
        }
    }
}
