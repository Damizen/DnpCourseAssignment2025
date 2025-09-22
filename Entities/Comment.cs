namespace Entities;

public class Comment(int Id, int User_Id, int Post_Id, string Body)
{
    public int Id { get; set; } = Id;
    public int User_Id { get; set; } = User_Id;
    public int Post_Id { get; set; } = Post_Id;
    public string Body { get; set; } = Body;
}