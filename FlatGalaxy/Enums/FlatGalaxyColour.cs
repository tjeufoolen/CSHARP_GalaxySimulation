namespace FlatGalaxy.Enums
{
    public enum FlatGalaxyColour
    {
        TRANSPARENT = 0,
        BLACK,
        ORANGE,
        PINK,
        PURPLE,
        BLUE,
        BROWN,
        GREY,
        YELLOW,
        GREEN,
        RED
    }

    // Idea from Reddit (https://www.reddit.com/r/csharp/comments/4ydcdy/is_there_some_way_to_override_tostring_for_enum/)
    static class Extensions
    {
        public static string ToHexColor(this FlatGalaxyColour colour) => colour switch
        {
            FlatGalaxyColour.BLACK => "#000000",
            FlatGalaxyColour.ORANGE => "#FFA500",
            FlatGalaxyColour.PINK => "#FFC0CB",
            FlatGalaxyColour.PURPLE => "#800080",
            FlatGalaxyColour.BLUE => "#0000ff",
            FlatGalaxyColour.BROWN => "#A52A2A",
            FlatGalaxyColour.GREY => "#808080",
            FlatGalaxyColour.YELLOW => "#FFFF00",
            FlatGalaxyColour.GREEN => "#00FF00",
            FlatGalaxyColour.RED => "#FF0000",
            _ => "#FFFFFF"
        };
    }
}
