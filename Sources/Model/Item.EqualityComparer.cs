namespace Model;

public partial class Item
{
    private sealed class FullEqComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item? x, Item? y)
        {
            return
                x is not null
                && y is not null
                && x.CreatedDate == y.CreatedDate
                && x.DisplayName == y.DisplayName
                && x.EnchantCategories.SequenceEqual(y.EnchantCategories)
                && x.Id == y.Id
                && x.ImageBase64 == y.ImageBase64
                && x.MaxDurability == y.MaxDurability
                && x.Name == y.Name
                && x.RepairWith.SequenceEqual(y.RepairWith)
                && x.StackSize == y.StackSize
                && x.UpdatedDate == y.UpdatedDate;
        }

        public int GetHashCode(Item obj) => string.GetHashCode(obj.Name);
    }
    
    public static IEqualityComparer<Item> FullComparer { get; } = new FullEqComparer();
}
