using System.Net;
using System.Text.Json;
using Model;
using RestController.Controllers;
using SF_Minecraft_Blazor.Services;
using Xunit;

namespace SF_Minecraft_Blazor_UT.Services;

public class DataItemListServiceUt
{
    #region Data

    public static IEnumerable<object[]> ListData()
    {
        yield return new object[]
        {
            1, 10, new CraftingController().List(1, 10).Result
        };
        yield return new object[]
        {
            0, 10, new CraftingController().List(0, 10).Result
        };
        yield return new object[]
        {
            -1, 10, new CraftingController().List(-1, 10).Result
        };
        yield return new object[]
        {
            int.MinValue, 10,  new CraftingController().List(int.MinValue, 10).Result
        };

        yield return new object[]
        {
            int.MaxValue, 10, new CraftingController().List(int.MaxValue, 10).Result
        };
        yield return new object[]
        {
            1, 0, new CraftingController().List(1, 0).Result
        };
        yield return new object[]
        {
            1, -1, new CraftingController().List(1, -1).Result
        };
        yield return new object[]
        {
            1, int.MinValue, new CraftingController().List(1, int.MinValue).Result
        };
        yield return new object[]
        {
            1, int.MaxValue, new CraftingController().List(1, int.MaxValue).Result
        };
        yield return new object[]
        {
            2, 5, new CraftingController().List(2, 5).Result
        };
        yield return new object[]
        {
            2, 15, new CraftingController().List(2, 15).Result
        };
    }

    public static IEnumerable<object?[]> GetByIdData()
    {
        yield return new object[]
        {
            1, new CraftingController().GetById(1).Result
        };
        yield return new object?[]
        {
            0, null
        };
        yield return new object?[]
        {
            -1, null
        };
        yield return new object?[]
        {
            int.MinValue, null
        };
        yield return new object?[]
        {
            int.MaxValue, null
        };
        yield return new object?[]
        {
            3, new CraftingController().GetById(3).Result
        };
        yield return new object?[]
        {
            100, new CraftingController().GetById(100).Result
        };
    }

    public static IEnumerable<object?[]> GetByNameData()
    {
        yield return new object[]
        {
            "stone_axe", new CraftingController().GetByName("stone_axe").Result
        };
        yield return new object?[]
        {
            "fqsldfkj", null
        };
    }

    public static IEnumerable<object[]> AddItemData()
    {
        yield return new object[]
        {
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = 0,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            new CraftingController().All().Result.Last().Id + 1
        };
        yield return new object[]
        {
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = -1,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            new CraftingController().All().Result.Last().Id + 1
        };
        yield return new object[]
        {
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = 1,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            new CraftingController().All().Result.Last().Id + 1
        };
        yield return new object[]
        {
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = 20,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            new CraftingController().All().Result.Last().Id + 1
        };
        yield return new object[]
        {
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = 450,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            new CraftingController().All().Result.Last().Id + 1
        };
    }

    public static IEnumerable<object[]> DeleteItemData()
    {
        yield return new object[]
        {
            1, true
        };
        yield return new object[]
        {
            0, false
        };
        yield return new object[]
        {
            -1, false
        };
        yield return new object[]
        {
            int.MinValue, false
        };
        yield return new object[]
        {
            int.MaxValue, false
        };
        yield return new object[]
        {
            3, true
        };
    }

    public static IEnumerable<object[]> UpdateItemData()
    {
        yield return new object[]
        {
            1,
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = 1,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            true
        };

        yield return new object[]
        {
            0,
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = 0,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            false
        };

        yield return new object[]
        {
            int.MaxValue,
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = 4,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            false
        };

        yield return new object[]
        {
            1,
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = int.MinValue,
                ImageBase64 = "Test",
                MaxDurability = 45,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            true
        };

        yield return new object[]
        {
            1,
            new Item
            {
                CreatedDate = DateTime.MinValue,
                DisplayName = "Test",
                EnchantCategories = new List<string>(),
                Id = int.MaxValue,
                ImageBase64 = "Test",
                MaxDurability = 0,
                Name = "Test",
                RepairWith = new List<string>(),
                StackSize = 0,
                UpdatedDate = null
            },
            true
        };
    }

    #endregion
    
    [Fact]
    public void TestConstructor() 
    {
        var service = new DataItemListService(
            TestUtils.GetHttpClientRead(
                new CraftingController(),
                "All",
                "all"),
            TestUtils.BaseUrl
        );
        
        Assert.NotNull(service);
    }

    [Fact]
    public async Task TestAll()
    {
        var expected = await new CraftingController().All();
        var service = new DataItemListService(
            TestUtils.GetHttpClientRead(
                new CraftingController(),
                "All",
                "Crafting/all"
            ), 
            TestUtils.BaseUrl
        );
        var result = (await service.All()).ToList();
        
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected, result, Item.FullComparer);
    }

    [Fact]
    public async Task TestCount()
    {
        var expected = await new CraftingController().Count();
        var service = new DataItemListService(
            TestUtils.GetHttpClientRead(
                new CraftingController(),
                "Count",
                "Crafting/count"
            ), 
            TestUtils.BaseUrl
        );
        var result = await service.Count();
       
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [MemberData(nameof(ListData))]
    public async Task TestList(int page, int pageSize, List<Item> expected)
    {
        var service = new DataItemListService(
            TestUtils.GetHttpClientRead(
                new CraftingController(),
                "List",
                $"Crafting/?currentPage={page}&pageSize={pageSize}",
                page,
                pageSize
            ), 
            TestUtils.BaseUrl
        );
        var result = (await service.List(page, pageSize)).ToList();
        
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected, result, Item.FullComparer);
    }
    
    [Theory]
    [MemberData(nameof(GetByIdData))]
    public async Task TestGetById(int id, Item? expected)
    {
        var service = new DataItemListService(
            TestUtils.GetHttpClientRead(
                new CraftingController(),
                "GetById",
                $"Crafting/{id}",
                id
            ), 
            TestUtils.BaseUrl
        );

        
        if (expected is null)
        { 
            await Assert.ThrowsAsync<JsonException>(async () => await service.GetById(id));
            return;
        }
        
        var result = await service.GetById(id);
        Assert.Equal(expected, result, Item.FullComparer);
    }
    
    [Theory]
    [MemberData(nameof(GetByNameData))]
    public async Task TestGetByName(string name, Item? expected)
    {
        var service = new DataItemListService(
            TestUtils.GetHttpClientRead(
                new CraftingController(),
                "GetByName",
                $"Crafting/by-name/{name}",
                name
            ), 
            TestUtils.BaseUrl
        );

        
        if (expected is null)
        {
            await Assert.ThrowsAsync<JsonException>(async () => await service.GetByName(name));
            return;
        }
        
        var result = await service.GetByName(name);
        Assert.Equal(expected, result, Item.FullComparer);
    }
    
    [Theory]
    [MemberData(nameof(AddItemData))]
    public async Task TestAddItem(Item item, int expectedId)
    {
        var controller = new CraftingController();
        var service = new DataItemListService(
            TestUtils.GetHttpClientWrite(
                controller,
               "Add",
               "Crafting/",
               typeof(Item),
               item
            ),
            TestUtils.BaseUrl
        );
        
        await service.Add(item);
        
        var result = controller.GetById(expectedId).Result;
        Assert.Equal(item, result, Item.FullComparer);

        await controller.ResetItems();
    }
    
    [Theory]
    [MemberData(nameof(DeleteItemData))]
    public async Task TestDeleteItem(int id, bool expected)
    {
        var controller = new CraftingController();
        var service = new DataItemListService(
            TestUtils.GetHttpClientWrite(
                controller,
                "Delete",
                $"Crafting/{id}",
                typeof(int),
                id
            ),
            TestUtils.BaseUrl
        );

        HttpStatusCode result;
        if (!expected)
        {
            result = await service.Delete(id);
            Assert.Equal(HttpStatusCode.InternalServerError, result);
            return;
        }
        
        result = await service.Delete(id);
        Assert.Equal(HttpStatusCode.OK, result);
        var error = await Assert.ThrowsAsync<ArgumentException>(async () => await controller.GetById(id));
        Assert.Equal($"Unable to found the item with ID: {id}", error.Message);
        
        await controller.ResetItems();
    }

    [Theory]
    [MemberData(nameof(UpdateItemData))]
    public async Task TestUpdateItem(int id, Item updatedItem, bool isNotFail)
    {
        var controller = new CraftingController();
        var service = new DataItemListService(
            TestUtils.GetHttpClientWrite(
                controller,
                "Update",
                $"Crafting/{id}",
                typeof(Item),
                id,
                updatedItem
            ),
            TestUtils.BaseUrl
        );
        
        HttpStatusCode result;
        if (!isNotFail)
        {
            result = await service.Update(id, updatedItem);
            Assert.Equal(HttpStatusCode.InternalServerError, result);
            return;
        }
        
        result = await service.Update(id, updatedItem);
        Assert.Equal(HttpStatusCode.OK, result);
        var resultItem = await controller.GetById(updatedItem.Id);
        
        Assert.Equal(updatedItem, resultItem, Item.FullComparer);
        
        await controller.ResetItems();
    }
    
    [Fact]
    public async Task TestResetItems()
    {
        var controller = new CraftingController();
        var expected = await controller.All();
        await controller.Delete(1);
        await controller.Delete(4);
        await controller.Delete(5);
        
        var service = new DataItemListService(
            TestUtils.GetHttpClientRead(
                controller,
                "ResetItems",
                "Crafting/reset-items"
            ),
            TestUtils.BaseUrl
        );
        
        await service.ResetItems();
        var result = await controller.All();
        
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected, result, Item.FullComparer);
    }
}
