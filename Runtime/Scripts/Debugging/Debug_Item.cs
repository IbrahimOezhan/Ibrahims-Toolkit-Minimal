using System.Text;

namespace IbrahKit
{
    public class Debug_Item
    {
        public StringBuilder content = new();
        public int order;

        public Debug_Item(int order)
        {
            this.order = order;
        }
    }
}