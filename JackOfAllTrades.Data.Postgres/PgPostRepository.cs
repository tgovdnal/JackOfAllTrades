using JackOfAllTrades.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JackOfAllTTrades.Data.Postgres
{
    internal class PgPostRepository(PgContext pgContext) : IPostRepository
    {
        public async Task<Post> AddPost(Post post)
        {
            await pgContext.Posts.AddAsync((PgPost)post);
            await pgContext.SaveChangesAsync();
            return post;
        }

        public async Task DeletePost(string slug)
        {
            var post = await pgContext.Posts.FirstOrDefaultAsync(p => p.Slug == slug);
            if (post != null)
            {
                pgContext.Posts.Remove(post);
                await pgContext.SaveChangesAsync();
            }
        }

        public async Task<Post?> GetPost(string dateString, string slug)
        {
            var theDate = DateTimeOffset.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);

            // get a post from the database based on the slug submitted
            var thePosts = await pgContext.Posts
                .Where(p => p.Slug == slug)
                .Select(p => (Post)p)
                .ToArrayAsync();

            return thePosts.FirstOrDefault(p =>
                p.PublishedDate.UtcDateTime.Date == theDate.UtcDateTime.Date);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await pgContext.Posts.ToArrayAsync();
            return posts.Select(p => (Post)p);
        }

        public async Task<IEnumerable<Post>> GetPosts(Expression<Func<Post, bool>> where)
        {
            return await pgContext.Posts.Where(p => where.Compile().Invoke((Post)p))
                    .Select(p => (Post)p)
                    .ToArrayAsync();
                    
        }

        public async Task<Post> UpdatePost(Post post)
        {
            // update a post in the database
            pgContext.Posts.Update((PgPost)post);
            await pgContext.SaveChangesAsync();

            return post;
        }
    }
}
