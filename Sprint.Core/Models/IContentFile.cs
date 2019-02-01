using System;

namespace Sprint.Models
{
    public interface IContentFile : IEquatable<IContentFile>
    {

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        string Filename { get; }

        /// <summary>
        /// Gets the slug.
        /// </summary>
        /// <value>
        /// The slug.
        /// </value>
        string Slug { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets the excerpt.
        /// </summary>
        /// <value>
        /// The excerpt.
        /// </value>
        string Excerpt { get; }

        /// <summary>
        /// Gets the permalink.
        /// </summary>
        /// <value>
        /// The permalink.
        /// </value>
        string Permalink { get; }

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        string Author { get; }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        DateTime? Date { get; }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        string Template { get; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        string Content { get; }

        /// <summary>
        /// To the HTML.
        /// </summary>
        /// <returns></returns>
        string ToHtml();
    }
}
