namespace ToDoApp.Models;

public static class ToDoItemsRepository
{
    private static List<ToDoItem> _items =
    [
        new() { Id = 1, Name = "Item 1" },
        new() { Id = 2, Name = "Item 2" },
        new() { Id = 3, Name = "Item 3" },
        new() { Id = 4, Name = "Item 4" },
        new() { Id = 5, Name = "Item 5" },
    ];

    public static void AddItem(ToDoItem item)
    {
        if (_items.Count > 0)
        {
            var maxId = _items.Max(i => i.Id);
            item.Id = maxId + 1;
        }
        else
        {
            item.Id = 1;
        }

        _items.Add(item);
    }

    public static List<ToDoItem> GetItems()
    {
        //OrderByDescending()：用于初始排序，对集合按指定键进行降序排列。
        //ThenByDescending()：必须跟在 OrderBy 或 ThenBy 之后，作为次级排序条件，对主排序后的结果继续按指定键降序排列。
        var sortedItem = _items.OrderBy(i => i.IsComplete).ThenByDescending(i => i.Id).ToList();
        return sortedItem;
    }

    public static ToDoItem? GetItemById(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        return item == null ? null : new ToDoItem() { Id = item.Id, Name = item.Name };
    }

    public static void UpdateItem(int itemId, ToDoItem item)
    {
        if (itemId != item.Id)
            return;

        var itemToUpdate = _items.FirstOrDefault(i => i.Id == itemId);
        if (itemToUpdate != null)
        {
            itemToUpdate.Name = item.Name;
        }
    }

    public static void DeleteServer(int itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _items.Remove(item);
        }
    }

    public static List<ToDoItem> SearchItems(string itemFilter)
    {
        return _items
            .Where(i =>
                i.Name != null && i.Name.Contains(itemFilter, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();
    }
}
