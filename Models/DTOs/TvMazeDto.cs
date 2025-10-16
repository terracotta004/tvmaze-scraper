using System.Text.Json.Serialization;

public record TvMazeShow(
    int Id,
    string Name,
    [property: JsonPropertyName("_embedded")] TvMazeEmbedded Embedded
);

public record TvMazeEmbedded(
    [property: JsonPropertyName("cast")] List<TvMazeCastMember> Cast
);

public record TvMazeCastMember(
    TvMazePerson Person,
    TvMazeCharacter Character,
    bool Self,
    bool Voice
);

public record TvMazePerson(int Id, string Name);
public record TvMazeCharacter(int Id, string Name);
