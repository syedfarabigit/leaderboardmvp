using Dapper;
using SiSU.Model;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SiSU.Infrastructure;
using System.Linq;

namespace SiSU.Repository
{
    public interface IUserRepository
    {
        void Create(string email, byte[] passwordHash, byte[] passwordSalt, List<UserRole> roles);
        User GetByEmail(string email);
        User GetById(int userId);
        IEnumerable<int> GetRoles(int userId);

    }

    public class UserRepository : IUserRepository
    {
        readonly string _connectionString;
        public UserRepository(IConfiguration iConfiguration)
        {
            _connectionString = iConfiguration.GetSection("ConnectionString").Value;
        }

        public void Create(string email, byte[] passwordHash, byte[] passwordSalt, List<UserRole> roles)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                var userId = connection.Query<int>(@"insert into [User](Email, PasswordHash, PasswordSalt) values(@email, @passwordHash, @passwordSalt); select cast(scope_identity() as int);", new { @email = email, @passwordHash = passwordHash, @passwordSalt = passwordSalt }).Single();
                if (roles != null && roles.Any())
                {
                    foreach (var role in roles)
                    {
                        connection.Execute(@"insert into [UserRole](UserId, RoleId) values(@userId, @roleId)", new { @userId = userId, @roleId = (int)role });
                    }
                }
            }
        }

        public User GetByEmail(string email)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>(@"select * from [User] where Email=@email", new { email });
            }
        }

        public User GetById(int userId)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>(@"select * from [User] where UserId=@userId", new { userId });
            }
        }

        public IEnumerable<int> GetRoles(int userId)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                return connection.Query<int>(@"select RoleId from [UserRole] where UserId=@userId", new { userId });
            }
        }

    }
}
