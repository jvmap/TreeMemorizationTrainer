using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App;

internal class DecisionTreeRootNode
{
    protected const string PLACEHOLDER = "##placeholder##";

    public string Answer { get; set; } = PLACEHOLDER;

    public List<DecisionTreeNode> Replies { get; } = [];
}
