using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace toh_dotnetcore.SSBlazor
{
    public class MessageState
    {
        public List<string> Messages { get; private set; } = new List<string>();
        public void AddMesage(string message)
        {
            Messages.Add($"HeroService: ${message}");
        }
        public void Clear()
        {
            Messages.Clear();
        }
    }
}
