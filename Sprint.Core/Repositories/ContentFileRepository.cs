using Sprint.IO;
using Sprint.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Repositories
{
    public class ContentFileRepository : IRepository<IContentFile>
    {
        private List<ContentFileData> lst = new List<ContentFileData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFileRepository" /> class.
        /// </summary>
        public ContentFileRepository()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFileRepository"/> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public ContentFileRepository(string folder)
            : this()
        {
            foreach (var fileInfo in Directories.FileList(folder, false))
            {
                lst.Add(new ContentFileData(new ContentFile(fileInfo.FullName), ContentFileStatus.Untouched));
            }
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Add(IContentFile entity)
        {
            lst.Add(new ContentFileData(entity, ContentFileStatus.Added));
        }

        /// <summary>
        /// Alls this instance.
        /// </summary>
        /// <returns></returns>
        public IQueryable<IContentFile> All()
        {
            return from p in lst.AsQueryable()
                   select p.ContentFile;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(IContentFile entity)
        {
            var data = lst.Where(p => p.ContentFile.Filename == entity.Filename).FirstOrDefault();

            if (data != null)
            {
                data.ContentFile = entity;
                data.Status = ContentFileStatus.Modified;
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(IContentFile entity)
        {
            var data = lst.Where(p => p.ContentFile.Filename == entity.Filename).FirstOrDefault();
            
            if (data != null)
            {
                data.Status = ContentFileStatus.Deleted;
            }
        }

        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public void Delete(Expression<Func<IContentFile, bool>> predicate)
        {
            var x = predicate.Compile();

            foreach (var item in lst.Where(p => x.Invoke(p.ContentFile).Equals(p.ContentFile)))
            {
                item.Status = ContentFileStatus.Deleted;
            }            
        }

        /// <summary>
        /// Filters the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="index">The index.</param>
        /// <param name="size">The size.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public IQueryable<IContentFile> Filter(Expression<Func<IContentFile, bool>> filter, int index, int size, out int total)
        {
            var x = filter.Compile();

            var d = lst.Where(p => x.Invoke(p.ContentFile).Equals(p.ContentFile))
                .Select(p => p.ContentFile)
                .Skip(index * size)
                .Take(size);

            total = d.Count();
            return d.AsQueryable();
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            foreach (var item in lst)
            {
                switch (item.Status)
                {
                    case ContentFileStatus.Added:
                        CreateFile(item.ContentFile);
                        break;
                    case ContentFileStatus.Modified:
                        UpdateFile(item.ContentFile);
                        break;
                    case ContentFileStatus.Deleted:
                        DeleteFile(item.ContentFile);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public IContentFile SingleOrDefault(Func<IContentFile, bool> predicate)
        {
            return lst.Where(p => p.ContentFile.Equals(predicate.Invoke(p.ContentFile))).Select(p => p.ContentFile).FirstOrDefault();          
        }

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="file">The file.</param>
        private void CreateFile(IContentFile file)
        {
            Sprint.IO.Files.WriteAllText(file.Filename, file.ToString(), false);
        }

        /// <summary>
        /// Updates the file.
        /// </summary>
        /// <param name="file">The file.</param>
        private void UpdateFile(IContentFile file)
        {
            DeleteFile(file);
            CreateFile(file);
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="file">The file.</param>
        private void DeleteFile(IContentFile file)
        {
            if (File.Exists(file.Filename))
                File.Delete(file.Filename);
        }
    }
}
