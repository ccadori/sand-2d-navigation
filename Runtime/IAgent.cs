using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Navigation
{
    public interface IAgent
    {
        INode CurrentNode { get; }
        INode OccupiedNode { get; }
    }
}
