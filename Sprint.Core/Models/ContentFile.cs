using Sprint.IO;
using Sprint.Serializers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace Sprint.Models
{
    public class ContentFile : DynamicObject, IContentFile
    {
        private const string parseSeparator = "---";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public ContentFile(string fileName)
            : this(fileName, "page.cshtml")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="template">The template.</param>
        public ContentFile(string fileName, string template)
        {
            Filename = fileName;
            Template = template;
            Parse();
        }

        /// <summary>
        /// Parses this instance.
        /// </summary>
        private void Parse()
        {
            if (!File.Exists(Filename))
            {
                return;
            }

            string str = Files.ReadAllText(Filename);

            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            string[] data = str.Split(new string[] { parseSeparator }, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length == 0)
            {
                return;
            }
            else
            {
                ParseHeader(data[0]);

                if (data.Length > 1)
                {
                    Content = data[1];
                }
            }
        }

        /// <summary>
        /// Parses the header.
        /// </summary>
        /// <param name="data">The data.</param>
        private void ParseHeader(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var jsonStr = YamlSerializer.Instance.SerializeToJson( YamlSerializer.Instance.Deserialize<dynamic>(data));
            var obj = JsonSerializer.Instance.Deserialize<dynamic>(jsonStr);
            
            if (obj == null)
            {
                return;
            }


            if (string.IsNullOrEmpty(Slug) && obj.slug != null)
            {
                Slug = obj.slug;
            }

            if (string.IsNullOrEmpty(Title) && obj.title != null)
            {
                Title = obj.title;
            }

            if (string.IsNullOrEmpty(Excerpt) && obj.excerpt != null)
            {
                Excerpt = obj.excerpt;
            }

            if (string.IsNullOrEmpty(Permalink) && obj.permalink != null)
            {
                Permalink = obj.permalink;
            }

            if (string.IsNullOrEmpty(Author) && obj.author != null)
            {
                Author = obj.author;
            }

            if (obj.template != null)
            {
                Template = obj.template;
            }

            if (Date == null && obj.date != null)
            {
                if (DateTime.TryParse(obj.date.ToString(), out DateTime dt))
                    Date = dt;
            }
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string Filename
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the slug.
        /// </summary>
        /// <value>
        /// The slug.
        /// </value>
        public string Slug
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the excerpt.
        /// </summary>
        /// <value>
        /// The excerpt.
        /// </value>
        public string Excerpt
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the permalink.
        /// </summary>
        /// <value>
        /// The permalink.
        /// </value>
        public string Permalink
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime? Date
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public string Template
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content
        {
            get;
            private set;
        }

        /// <summary>
        /// To the HTML.
        /// </summary>
        /// <returns></returns>
        public string ToHtml()
        {
            Markdown.Markdown markdown = new Markdown.Markdown()
            {
                AutoHyperlink = true,
                AutoNewLines = true,
                LinkEmails = true,
                EncodeProblemUrlCharacters = true,
                StrictBoldItalic = true,
                Prettify = true,
                EmptyElementSuffix = "/>"
            };

            return markdown.Transform(Content);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("---");

            foreach (var item in (IDictionary<string, object>)this)
            {
                sb.AppendLine($"{item.Key}: {item.Value}");
            }

            sb.AppendLine("---");
            sb.AppendLine("");
            sb.AppendLine(Content);

            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals((IContentFile)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(IContentFile other)
        {
            if (this.GetHashCode() == other.GetHashCode())
            {
                return true;
            }

            if (this.Filename.Equals(other.Filename, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
