using Microsoft.AspNetCore.Mvc;
using NewsPortalIIT.Models;

namespace NewsPortalIIT.Controllers;

public class BaseController : ControllerBase
{
    protected static readonly System.Text.Json.JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
    };

    private const string DbPath = "db.json";

    protected DbData GetDbData()
    {
        if (!System.IO.File.Exists(DbPath))
        {
            return new DbData { News = new List<News>() };
        }

        var json = System.IO.File.ReadAllText(DbPath);
        try
        {
            var data = System.Text.Json.JsonSerializer.Deserialize<DbData>(json, _jsonOptions);
            return data ?? new DbData { News = new List<News>() };
        }
        catch
        {
            return new DbData { News = new List<News>() };
        }
    }

    protected void SaveDbData(DbData data)
    {
        var updatedJson = System.Text.Json.JsonSerializer.Serialize(data, _jsonOptions);
        System.IO.File.WriteAllText(DbPath, updatedJson);
    }
}
