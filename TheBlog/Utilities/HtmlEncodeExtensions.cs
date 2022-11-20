using System.Web;
using TheBlog.DataAccess;

namespace TheBlog.Utilities
{
    /// <summary>
    /// Extension methods for HTML Encode blog post entities
    /// </summary>
    public static class HtmlEncodeExtensions
    {
        /// <summary>
        /// HTML encodes one blog post entity.
        /// Accpeted HTML tags are:  "<b>", "</b>", "<i>", "</i>"
        /// </summary>
        /// <param name="blogPost">The <see cref="BlogPostEntity"> to HTML encode</param>
        public static void HtmlEncode(this BlogPostEntity blogPost)
        {
            string[] _allowedTags = new string[] { "<b>", "</b>", "<i>", "</i>", "å", "Å", "ä", "Ä", "ö", "Ö"  };

            // HTML encodes all the text in the blog post
            string encodedText = HttpUtility.HtmlEncode(blogPost.Text);

            // Replaces the already encoded accepted HTML tags
            foreach (var tag in _allowedTags)
            {
                var encodedTag = HttpUtility.HtmlEncode(tag);
                encodedText = encodedText.Replace(encodedTag, tag);
            }

            // Replaces the blog post text with the encoded text
            blogPost.Text = encodedText;
        }

        /// <summary>
        /// HTML encodes all blog post entities.
        /// Accpeted HTML tags are:  "<b>", "</b>", "<i>", "</i>"
        /// </summary>
        /// <param name="blogPosts">The <see cref="IEnumerable<BlogPostEntity>"> to HTML encode</param>
        public static void HtmlEncode(this IEnumerable<BlogPostEntity> blogPosts)
        {
            foreach (var blogPost in blogPosts)
            {
                blogPost.HtmlEncode();
            }
        }
    }
}