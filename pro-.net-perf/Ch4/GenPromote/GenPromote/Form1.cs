using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenPromote
{
    public partial class Form1 : Form
    {
        /*
        Gen0:新規作成オブジェクトは全てGen0に収容。メモリ空間256KB～8MB(キャッシュサイズの影響を受ける)。
             Gen0が満杯になるとGen0を対象としたGC実行。非常に高速。
        Gen1:1回のGCで解放されなかったオブジェクトを収容。メモリ空間 512KB～4MB。
             Gen1が満杯になるとGen1を対象としたGC実行。比較的高速。
        Gen2:2回以上のGCで解放されなかったオブジェクトを収容する最終メモリ領域。しばらくGCの対象にならない。 メモリ空間制限なし。
             Gen2が満杯になるとシステムが停止するため、他と異なり動的な閾値によってGC実行。他に比べると桁違いの遅さ。

        ※全てFullGC
        ■GC回数0
        [Normal]Gen：0
        [Pinned]Gen：0
        [Weak]Gen：0
        [WeakTrackResurrection]Gen：0
        GC.Collect = 13121tick
        ■GC回数1
        [Normal]Gen：1
        [Pinned]Gen：0
        [Weak]Gen：0
        [WeakTrackResurrection]Gen：1
        GC.Collect = 13776tick
        ■GC回数2
        [Normal]Gen：2
        [Pinned]Gen：0
        [Weak]Gen：0
        [WeakTrackResurrection]Gen：2
        GC.Collect = 11898tick
         */
        public Form1()
        {
            InitializeComponent();
            LatencyModeLabel.Text = GCSettings.LatencyMode.ToString();
        }

        private void button_Click(object sender, EventArgs e)
        {
            byte[][] datas = new byte[][] { new byte[128], new byte[128], new byte[128], new byte[128] };
            GCHandleType[] gcHandleTypes = new GCHandleType[] { GCHandleType.Normal, GCHandleType.Pinned, GCHandleType.Weak, GCHandleType.WeakTrackResurrection };
            List<GCHandle> gcHandles = new List<GCHandle>();
            StringBuilder sb = new StringBuilder();

            int gcCount = 3;
            Stopwatch sw = new Stopwatch();

            for (int j = 0; j < gcHandleTypes.Length; j++)
            {
                gcHandles.Add(GCHandle.Alloc(datas[j], gcHandleTypes[j]));
            }

            for (int i = 0; i < gcCount; i++)
            {
                sb.AppendLine($"■GC回数{i}");
                for (int j = 0; j < gcHandleTypes.Length; j++)
                {
                    sb.AppendLine($"[{gcHandleTypes[j].ToString()}]Gen：" + GC.GetGeneration(datas[j]).ToString());
                }
                sw.Restart();
                GC.Collect();

                sb.AppendLine($"GC.Collect = {sw.ElapsedTicks}tick");
            }

            gcHandles.ForEach(item => item.Free());

            MessageBox.Show(sb.ToString());

        }
    }
}
