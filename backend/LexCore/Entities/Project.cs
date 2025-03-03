using LexCore.ServiceInterfaces;

namespace LexCore.Entities;

public class Project : EntityBase
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required RetentionPolicy RetentionPolicy { get; set; }
    public required ProjectType Type { get; set; }
    public required List<ProjectUsers> Users { get; set; }
    public required DateTimeOffset? LastCommit { get; set; }

    public async Task<Changeset[]> GetChangesets(IHgService hgService)
    {
        return await hgService.GetChangesets(Code);
    }
}

public enum ProjectType
{
    Unknown = 0,
    FLEx = 1,
    WeSay = 2,
    OneStoryEditor = 3,
    OurWord = 4
}

public class Changeset
{
    public string Node { get; set; }
    public double[] Date { get; set; }
    public string Desc { get; set; }

    public string Branch { get; set; }

// commented out because I'm not sure of the shape and you can't use JsonArray as an output of gql
    // public JsonArray Bookmarks { get; set; }
    public string[] Tags { get; set; }
    public string User { get; set; }
    public string Phase { get; set; }
    public string[] Parents { get; set; }
}
