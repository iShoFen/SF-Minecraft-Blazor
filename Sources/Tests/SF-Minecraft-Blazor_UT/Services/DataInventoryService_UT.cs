using System.Net;
using Model.Inventory;
using RestController.Controllers;
using SF_Minecraft_Blazor.Services;
using Xunit;

namespace SF_Minecraft_Blazor_UT.Services;

public class DataInventoryServiceUt
{

    #region data

    public static IEnumerable<object[]> AddData()
    {
        yield return new object[]
        {
            new InventoryModel
            {
                ItemId = 1,
                NumberItem = 1,
                Position = 1
            }
        };
        yield return new object[]
        {
            new InventoryModel
            {
                ItemId = -1,
                NumberItem = -1,
                Position = -1
            }
        };
        yield return new object[]
        {
            new InventoryModel
            {
                ItemId = int.MinValue,
                NumberItem = int.MinValue,
                Position = int.MinValue
            }
        };
        yield return new object[]
        {
            new InventoryModel
            {
                ItemId = int.MaxValue,
                NumberItem = int.MaxValue,
                Position = int.MaxValue
            }
        };
    }
    
    public static IEnumerable<object[]> DeleteData()
    {
        yield return new object[]
        {
            1, true
        };
        yield return new object[]
        {
            -1, false
        };
    }

    public static IEnumerable<object?[]> UpdateData()
    {
        yield return new object?[]
        {
            null,
            new InventoryModel
            {
                ItemId = 1,
                NumberItem = 1,
                Position = 1
            },
            false
        };
        yield return new object[]
        {
            new InventoryModel
            {
                ItemId = 15,
                NumberItem = 15,
                Position = 15
            },
            new InventoryModel
            {
                ItemId = 1,
                NumberItem = 15,
                Position = 15
            },
            false
        };
        yield return new object[]
        {
            new InventoryModel
            {
                ItemId = 15,
                NumberItem = 15,
                Position = 15
            },
            new InventoryModel
            {
                ItemId = 15,
                NumberItem = 15,
                Position = 1
            },
            false
        };
        yield return new object[]
        {
            new InventoryModel
            {
                ItemId = int.MaxValue,
                NumberItem = 15,
                Position = int.MaxValue
            },
            new InventoryModel
            {
                ItemId = int.MaxValue,
                NumberItem = int.MaxValue,
                Position = int.MaxValue
            },
            true
        };
    }

    #endregion

    [Fact]
    public void TestConstructor()
    {
        var service = new DataInventoryService(
            TestUtils.GetHttpClientRead(
                new InventoryController(),
                "GetInventory",
                "Inventory/"),
            TestUtils.BaseUrl
        );
        
        Assert.NotNull(service);
    }
    
    [Fact]
    public async Task TestGetInventory()
    {
        var controller = new InventoryController();
        var expected = await controller.GetInventory();
        var service = new DataInventoryService(
            TestUtils.GetHttpClientRead(
                new InventoryController(),
                "GetInventory",
                "Inventory/"),
            TestUtils.BaseUrl
        );
        
        var result = await service.GetInventory();
        
        Assert.Equal(expected.Count, result.Count);
        Assert.Equal(expected, result, InventoryModel.FullComparer);
    }
    
    [Theory]
    [MemberData(nameof(AddData))]
    public async Task TestAddInventory(InventoryModel item)
    {
        var controller = new InventoryController();
        var expected = await controller.GetInventory();
        var service = new DataInventoryService(
            TestUtils.GetHttpClientWrite(
                new InventoryController(),
                "AddToInventory",
                "Inventory/",
                typeof(InventoryModel),
                item),
            TestUtils.BaseUrl
        );
        
        await service.AddToInventory(item);
        var result = await controller.GetInventory();
        
        Assert.Equal(expected.Count + 1, result.Count);
        Assert.Contains(item, result, InventoryModel.FullComparer);
        
        await controller.DeleteFromInventory(item.Position);
    }

    [Theory]
    [MemberData(nameof(DeleteData))]
    public async Task TestDeleteInventory(int position, bool expected)
    {
        var controller = new InventoryController();
        if (expected)
        {
            await controller.AddToInventory(new InventoryModel
            {
                ItemId = 1,
                NumberItem = 1,
                Position = position
            });
        }
        
        var service = new DataInventoryService(
            TestUtils.GetHttpClientWrite(
                new InventoryController(),
                "DeleteFromInventory",
                $"Inventory/{position}",
                typeof(int),
                position),
            TestUtils.BaseUrl
        );


        HttpStatusCode result;
        if (!expected)
        {
            result = await service.DeleteFromInventory(position);
            Assert.Equal(HttpStatusCode.InternalServerError, result);
            return;
        }
        
        result = await service.DeleteFromInventory(position);
        Assert.Equal(HttpStatusCode.OK, result);
        
        Assert.DoesNotContain(await controller.GetInventory(), 
            i => i.Position == position);
    }

    [Theory]
    [MemberData(nameof(UpdateData))]
    public async Task TestUpdateInventory(InventoryModel? baseItem, InventoryModel updatedItem, bool expected)
    {
        var controller = new InventoryController();
        if (baseItem != null)
        {
            await controller.AddToInventory(baseItem);
        }   
        
        var service = new DataInventoryService(
            TestUtils.GetHttpClientWrite(
                new InventoryController(),
                "UpdateInventory",
                "Inventory/",
                typeof(InventoryModel),
                updatedItem),
            TestUtils.BaseUrl
        );
        
        HttpStatusCode result;
        if (!expected)
        {
            result = await service.UpdateInventory(updatedItem);
            Assert.Equal(HttpStatusCode.InternalServerError, result);
            if (baseItem != null) await controller.DeleteFromInventory(baseItem.Position);
            return;
        }
        
        result = await service.UpdateInventory(updatedItem);
        Assert.Equal(HttpStatusCode.OK, result);
        
        var resultItem = await controller.GetInventory();
        Assert.Contains(updatedItem, resultItem, InventoryModel.FullComparer);
        
        await controller.DeleteFromInventory(updatedItem.Position);
    }
}
