using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly DataContext _dbContext; 
        public UserServices(DataContext context)
        {
            _dbContext = context;
        }
        
        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.Include(u => u.Ratings).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            User? user = await _dbContext.Users.FindAsync(id);
            if (user == null) { return null; }
            return user;
        }

        public async Task<bool> AddUser(User user)
        {

            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw new Exception("Error: Could Not Add User");
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            User? userToRemove = await _dbContext.Users.FindAsync(id);
            if(userToRemove == null)
            {
                return false;
            }
            _dbContext.Users.Remove(userToRemove);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditUser(User user)
        {
            User? userToEdit = await _dbContext.Users.FindAsync(user.Id);
            if (userToEdit == null) { return false; }
            
            userToEdit.UserName = user.UserName;
            userToEdit.UserEmail = user.UserEmail;
            userToEdit.Password = user.Password;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
