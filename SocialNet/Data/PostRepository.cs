﻿using Microsoft.EntityFrameworkCore;
using SocialNet.Data.Interfaces;
using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNet.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Post> CreatePostAsync(Post post)
        {
            post.Id = Guid.NewGuid();
            _context.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public Post CreatePost(Post post)
        {
            post.Id = Guid.NewGuid();
            _context.Add(post);
            _context.SaveChanges();
            return post;
        }
        public async Task<IEnumerable<Post>> GetPostsAsync()
            => await _context.Posts.Include(p => p.OriginalPoster).ToListAsync();

    }
}
