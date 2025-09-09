namespace Entities;

public class Comment(int id, int userId, int postId, string body)
{
    public int Id {get; set;} = id;
    public int User_Id {get; set;} = userId;
    public int Post_Id {get; set;} = postId;
    public string Body {get; set;} = body;
}