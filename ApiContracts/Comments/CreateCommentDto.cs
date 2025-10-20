namespace ApiContracts.Comments;

public class CreateCommentDto
{
    public int User_Id { get; set; }
    public int Post_Id { get; set; }
    public string Body { get; set; }
}