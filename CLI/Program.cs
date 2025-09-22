using FileRepositories;
using RepositoryContracts;
using UI;

var userRepo = new UserFileRepository();
var postRepo = new PostFileRepository();
var commentRepo = new CommentFileRepository();

var app = new CliApp(userRepo, postRepo, commentRepo);
await app.StartAsync();