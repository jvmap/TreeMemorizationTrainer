using System.Text.Json;

namespace App
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine($"Usage: {args[0]} <filename.json>");
                return 2;
            }
            string fileName = args[1];
            using Stream utf8Json = File.OpenRead(fileName);
            DecisionTreeRootNode? tree = JsonSerializer
                .Deserialize<DecisionTreeRootNode>(utf8Json);
            if (tree == null)
            {
                Console.WriteLine("Empty decision tree.");
                return 2;
            }
            return Walk(tree);
        }

        private static int Walk(DecisionTreeRootNode rootNode)
        {
            Console.WriteLine("How do you start?");
            return HandleUserInput(rootNode);
        }

        private static int HandleUserInput(DecisionTreeRootNode node)
        {
            string? userInput = Console.ReadLine();
            if (userInput == null)
            {
                Console.WriteLine("Exiting.");
                return -1;
            }
            if (userInput == node.Answer)
            {
                return Walk(node.Replies);
            }
            else
            {
                Console.WriteLine($"Wrong! {node.Answer} was correct.");
                return 1;
            }
        }

        private static int Walk(List<DecisionTreeNode> replies)
        {
            if (replies.Count == 0)
            {
                Console.WriteLine("Correct.");
                return 0;
            }
            DecisionTreeNode node = replies[Random.Shared.Next(replies.Count)];
            Console.WriteLine(node.Reply + ". What now?");
            return HandleUserInput(node);
        }
    }
}
