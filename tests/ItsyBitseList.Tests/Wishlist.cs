namespace ItsyBitseList.Tests
{
    // create a Wishlist class according to the tests
    internal class Wishlist
    {
        public Wishlist(string name, string owner)
        {
            Name = name;
            Owner = owner;
            Items = new List<string>();
        }

        public string Name { get; }
        public string Owner { get; }
        public IEnumerable<string> Items { get; internal set; }
    }

 

}