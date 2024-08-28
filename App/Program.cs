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
            do
            {
                result = Walk(tree);
                Console.WriteLine();
            } while (result >= 0);
            return result;
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
