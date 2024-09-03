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
            int mistakes = 0;
            Stack<DecisionTreeRootNode> path = [];
            do
            {
                result = Walk(tree, path, ref mistakes);
                Console.WriteLine();
            } while (result >= 0);
            return result;
        }

        private static int Walk(
            DecisionTreeRootNode rootNode, 
            Stack<DecisionTreeRootNode> path,
            ref int mistakes)
        {
            int count = CountOptions(rootNode);
            Console.WriteLine($"There are {count} challenges remaining.");
            Console.WriteLine("How do you start?");
            path.Push(rootNode);
            int result = HandleUserInput(rootNode, path, ref mistakes);
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
            Stack<DecisionTreeRootNode> path,
            ref int mistakes)
        {
            string? userInput = Console.ReadLine();
            if (userInput == null)
            {
                Console.WriteLine("Exiting.");
                return -1;
            }
            if (userInput == node.Answer)
            {
                return Walk(node.Replies, path, ref mistakes);
            }
            else
            {
                Console.WriteLine($"Wrong! {node.Answer} was correct.");
                mistakes++;
                return 1;
            }
        }

        private static int Walk(
            List<DecisionTreeNode> replies,
            Stack<DecisionTreeRootNode> path,
            ref int mistakes)
        {
            if (replies.Count == 0)
            {
                Console.WriteLine("Correct.");
                return PruneTree(path, mistakes);
            }
            DecisionTreeNode node = replies[Random.Shared.Next(replies.Count)];
            path.Push(node);
            Console.WriteLine(node.Reply + ". What now?");
            return HandleUserInput(node, path, ref mistakes);
        }

        private static int PruneTree(Stack<DecisionTreeRootNode> path, int mistakes)
        {
            DecisionTreeRootNode? current;
            do
            {
                DecisionTreeRootNode itemToRemove = path.Pop();
                if (!path.TryPeek(out current))
                {
                    Console.WriteLine();
                    Console.WriteLine("Completed!");
                    Console.WriteLine($"{mistakes} mistakes were made.");
                    return -1;
                }
                current.Replies.Remove((DecisionTreeNode)itemToRemove);
            } while (current.Replies.Count == 0);
            return 0;
        }
    }
}
