using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ch4.Listing1
{
    public class BodyRenderer : IRenderer
    {
        public string Render(Message message)
        {
            return $"<b>{message.Body}</b>";
        }
    }
}
