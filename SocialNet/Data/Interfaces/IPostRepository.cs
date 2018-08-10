using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNet.Data.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreatePostAsync(Post post);
        Task<IEnumerable<Post>> GetPostsAsync();
    }
}
