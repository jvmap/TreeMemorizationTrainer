using Newtonsoft.Json;

namespace App
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: App <filename.json>");
                return 2;
            }
            string fileName = args[0];
            string jsonString = File.ReadAllText(fileName);
            DecisionTreeRootNode? tree = JsonConvert.DeserializeObject<DecisionTreeRootNode>(jsonString);
            if (tree == null)
            {
                Console.WriteLine("Empty decision tree.");
                return 2;
            }
            int result;
            Stack<DecisionTreeRootNode> path = [];
            do
            {
                result = Walk(tree, path);
                Console.WriteLine();
            } while (result >= 0);
            return result;
        }

        private static int Walk(
            DecisionTreeRootNode rootNode, 
            Stack<DecisionTreeRootNode> path)
        {
            int count = CountOptions(rootNode);
            Console.WriteLine($"There are {count} challenges remaining.");
            Console.WriteLine("How do you start?");
            path.Push(rootNode);
            int result = HandleUserInput(rootNode, path);
            path.Clear();
            return result;
        }

        private static int CountOptions(DecisionTreeRootNode rootNode)
        {
            if (rootNode.Replies.Count == 0) return 1;
            int options = 0;
            foreach (var reply in rootNode.Replies)
            {
                options += CountOptions(reply);
            }
            return options;
        }

        private static int HandleUserInput(
            DecisionTreeRootNode node, 
            Stack<DecisionTreeRootNode> path)
        {
            string? userInput = Console.ReadLine();
            if (userInput == null)
            {
                Console.WriteLine("Exiting.");
                return -1;
            }
            if (userInput == node.Answer)
            {
                return Walk(node.Replies, path);
            }
            else
            {
                Console.WriteLine($"Wrong! {node.Answer} was correct.");
                return 1;
            }
        }

        private static int Walk(
            List<DecisionTreeNode> replies,
            Stack<DecisionTreeRootNode> path)
        {
            if (replies.Count == 0)
            {
                Console.WriteLine("Correct.");
                return PruneTree(path);
            }
            DecisionTreeNode node = replies[Random.Shared.Next(replies.Count)];
            path.Push(node);
            Console.WriteLine(node.Reply + ". What now?");
            return HandleUserInput(node, path);
        }

        private static int PruneTree(Stack<DecisionTreeRootNode> path)
        {
            DecisionTreeRootNode? current;
            do
            {
                DecisionTreeRootNode itemToRemove = path.Pop();
                if (!path.TryPeek(out current))
                {
                    Console.WriteLine();
                    Console.WriteLine("Completed!");
                    return -1;
                }
                current.Replies.Remove((DecisionTreeNode)itemToRemove);
            } while (current.Replies.Count == 0);
            return 0;
        }
    }
}
