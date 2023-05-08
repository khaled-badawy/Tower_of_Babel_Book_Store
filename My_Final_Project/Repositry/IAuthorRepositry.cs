using book_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Repositry
{
    public interface IAuthorRepositry
    {
        public Author GetById(int id);
        public List<Author> GetAll();
        public Task New([Bind("Name, BriefHistory, PublishCount")] Author author, IFormFile file);
        public Task Edit(int id, [Bind("Id,Name, BriefHistory, PublishCount")] Author Newauthor, IFormFile file);
        public void Delete(int id);
	}
}
