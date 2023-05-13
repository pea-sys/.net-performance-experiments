using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ch4.Listing1
{
    public class HeaderRenderer : IRenderer
    {
        public string Render(Message message)
        {
            return $"<h1>{message.Header}</h1>";
        }
    }
}
