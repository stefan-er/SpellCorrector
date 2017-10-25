/*
The MIT License (MIT)
Copyright (c) 2013
 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SpellCorrector.Web.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;

    public class BKTree
    {
        private Node root;
        private ILevenshteinDistanceAlgorithm levenshteinDistanceAlgorithm;

        public BKTree(ILevenshteinDistanceAlgorithm levenshteinDistanceAlgorithm)
        {
            this.levenshteinDistanceAlgorithm = levenshteinDistanceAlgorithm;
        }

        public void Add(string word)
        {
            word = word.ToLower();
            if (this.root == null)
            {
                this.root = new Node(word);
                return;
            }

            var currentNode = this.root;

            var distance = levenshteinDistanceAlgorithm.GetDistance(currentNode.Word, word);
            while (currentNode.ContainsKey(distance))
            {
                if (distance == 0)
                    return;

                currentNode = currentNode[distance];
                distance = levenshteinDistanceAlgorithm.GetDistance(currentNode.Word, word);
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
            var curDist = levenshteinDistanceAlgorithm.GetDistance(node.Word, word);
            var minDist = curDist - d;
            var maxDist = curDist + d;

            if (curDist <= d)
                rtn.Add(node.Word);

            foreach (var key in node.Keys.Cast<int>().Where(key => minDist <= key && key <= maxDist))
            {
                RecursiveSearch(node[key], rtn, word, d);
            }
        }
    }
}
