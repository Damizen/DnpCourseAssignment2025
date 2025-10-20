namespace ApiContracts.Comments;

public class CommentDto
{
    public int Id { get; set; }
    public int User_Id { get; set; }
    public int Post_Id { get; set; }
    public string Body { get; set; }
}