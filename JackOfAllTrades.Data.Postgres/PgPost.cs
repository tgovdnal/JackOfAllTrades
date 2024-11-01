using JackOfAllTrades.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackOfAllTTrades.Data.Postgres
{
    public class PgPost
    {
        [Required, Key, MaxLength(300)]
        public required string Slug { get; set; }

        [Required, MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; } = string.Empty;

        [Required]
        public required DateTimeOffset Published { get; set; } = DateTimeOffset.MaxValue;

        public static explicit operator PgPost(Post post)
        {

            return new PgPost
            {
                Slug = post.Slug,
                Title = post.Title,
                Content = post.Content,
                Published = post.PublishedDate
            };

        }

        public static explicit operator Post(PgPost post)
        {

            return new Post
            {
                Slug = post.Slug,
                Title = post.Title,
                Content = post.Content,
                PublishedDate = post.Published
            };

        }
    }
}
