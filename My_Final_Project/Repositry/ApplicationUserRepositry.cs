using My_Final_Project.Models;

namespace My_Final_Project.Repositry
{
    public class ApplicationUserRepositry : IApplicationUserRepositry
    {
        private readonly StoreContext StoreContext;
        public ApplicationUserRepositry(StoreContext storeContext)
        {
            this.StoreContext = storeContext;
        }
        public List<ApplicationUser> GetAll()
        {
            return StoreContext.Users.ToList();
        }
        public ApplicationUser Get(string id)
        {
            var user = StoreContext.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }
        public void Delete(string id)
        {
            var oldUser = Get(id);
            var userRole = StoreContext.UserRoles.FirstOrDefault(x => x.UserId == oldUser.Id);

            StoreContext.Users.Remove(oldUser);
            StoreContext.UserRoles.Remove(userRole);

            StoreContext.SaveChanges();
        }
    }
}
