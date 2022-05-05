using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCalledByNullConditionalOperator
{
    public class EventSource
    {
        public EventHandler<int> Updated;

        /// <summary>
        /// イベントハンドラを呼び出します
        /// </summary>
        public void RaiseUpdates()
        {
            counter++;
            Updated(this, counter);
        }

        public void RaiseUpdatesNullable()
        {
            counter++;
            Updated?.Invoke(this, counter);
        }
        private int counter;
    }
}
