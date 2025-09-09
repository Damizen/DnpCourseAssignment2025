namespace Entities;

public class Post(int id, int userId, string title, string body)
{
    public int Id { get; set; } = id;
    public int User_Id { get; set; } = userId;
    public string Title  { get; set; } = title;
    public string Body { get; set; } = body;
}