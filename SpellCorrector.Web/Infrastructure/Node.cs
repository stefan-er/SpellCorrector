namespace SpellCorrector.Web.Infrastructure
{
    using System.Collections;
    using System.Collections.Specialized;

    public class Node
    {
        public Node()
        {
        }

        public Node(string word)
        {
            this.Word = word.ToLower();
        }

        public string Word { get; set; }
        public HybridDictionary Children { get; set; }

        public Node this[int key]
        {
            get { return (Node)Children[key]; }
        }

        public ICollection Keys
        {
            get
            {
                if (Children == null)
                    return new ArrayList();
                return Children.Keys;
            }
        }

        public bool ContainsKey(int key)
        {
            return Children != null && Children.Contains(key);
        }

        public void AddChild(int key, string word)
        {
            if (this.Children == null)
                Children = new HybridDictionary();
            this.Children[key] = new Node(word);
        }
    }
}
