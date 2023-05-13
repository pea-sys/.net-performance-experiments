using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ch4.Listing1
{
    public class FooterRenderer : IRenderer
    {
        public string Render(Message message)
        {
            return $"<i>{message.Footer}</i>";
        }
    }
}
