using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App;

internal class DecisionTreeNode : DecisionTreeRootNode
{
    public string Reply { get; set; } = PLACEHOLDER;

    public override string ToString()
    {
        return $"{Reply} {Answer}";
    }
}
