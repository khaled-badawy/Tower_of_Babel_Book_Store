using My_Final_Project.Models;

namespace My_Final_Project.Repositry
{

        public interface IApplicationUserRepositry
        {
            public List<ApplicationUser> GetAll();
            public void Delete(string id);
            public ApplicationUser Get(string id);
        }

}
