namespace Model.Inventory;

public partial class InventoryModel
{
    private sealed class FullEqComparer : IEqualityComparer<InventoryModel>
    {
        public bool Equals(InventoryModel? x, InventoryModel? y)
        {
            return
                x is not null
                && y is not null
                && x.ItemId == y.ItemId
                && x.NumberItem == y.NumberItem
                && x.Position == y.Position;
        }

        public int GetHashCode(InventoryModel obj) => obj.Position % 15;
    }
    
    public static IEqualityComparer<InventoryModel> FullComparer { get; } = new FullEqComparer();
}
