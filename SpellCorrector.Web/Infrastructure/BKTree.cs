namespace SpellCorrector.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BKTree
    {
        private Node root;

        public void Add(string word)
        {
            word = word.ToLower();
            if (this.root == null)
            {
                this.root = new Node(word);
                return;
            }

            var currentNode = this.root;

            var distance = LevenshteinDistance(currentNode.Word, word);
            while (currentNode.ContainsKey(distance))
            {
                if (distance == 0)
                    return;

                currentNode = currentNode[distance];
                distance = LevenshteinDistance(currentNode.Word, word);
            }

            currentNode.AddChild(distance, word);
        }

        public List<string> Search(string word, int d)
        {
            var rtn = new List<string>();
            word = word.ToLower();

            RecursiveSearch(this.root, rtn, word, d);

            return rtn;
        }

        private void RecursiveSearch(Node node, List<string> rtn, string word, int d)
        {
            var curDist = LevenshteinDistance(node.Word, word);
            var minDist = curDist - d;
            var maxDist = curDist + d;

            if (curDist <= d)
                rtn.Add(node.Word);

            foreach (var key in node.Keys.Cast<int>().Where(key => minDist <= key && key <= maxDist))
            {
                RecursiveSearch(node[key], rtn, word, d);
            }
        }

        public static int LevenshteinDistance(string first, string second)
        {
            if (first.Length == 0) return second.Length;
            if (second.Length == 0) return first.Length;

            var lenFirst = first.Length;
            var lenSecond = second.Length;

            var d = new int[lenFirst + 1, lenSecond + 1];

            for (var i = 0; i <= lenFirst; i++)
                d[i, 0] = i;

            for (var i = 0; i <= lenSecond; i++)
                d[0, i] = i;

            for (var i = 1; i <= lenFirst; i++)
            {
                for (var j = 1; j <= lenSecond; j++)
                {
                    var match = (first[i - 1] == second[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + match);
                }
            }

            return d[lenFirst, lenSecond];
        }
    }
}
