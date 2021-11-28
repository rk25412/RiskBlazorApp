using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Clear.Risk.Data;
using Clear.Risk.Models;
using Clear.Risk.Models.ClearConnection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Clear.Risk
{
    public partial class Startup
    {
        partial void OnConfigureServices(IServiceCollection services)
        {
            // Replace the default ASP.NET Identity implementations with the custom ones
            services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();
            services.AddTransient<IRoleStore<IdentityRole>, CustomRoleStore>();
            services.AddTransient<IPasswordHasher<ApplicationUser>, CustomPasswordHasher>();
        }
    }

    public class CustomPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        // Hashes passwords using the SHA256 algorithm. You can use your existing hashing algorithm.
        public string HashPassword(ApplicationUser user, string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }

        // Compares two hashed passwords
        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            var providedHashedPasswored = HashPassword(user, providedPassword);

            if (hashedPassword == providedHashedPasswored)
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
            //return PasswordVerificationResult.Success;
        }

        // Role management - mostly CRUD operations
       
    }

    public class CustomRoleStore : IQueryableRoleStore<IdentityRole>
    {
        private readonly ClearConnectionContext context;

        public CustomRoleStore(ClearConnectionContext context)
        {
            this.context = context;
        }

        // Maps Role instances (custom) to IdentityRole instances (ASP.NET Core Identity)
        public IQueryable<IdentityRole> Roles
        {
            get
            {
                return context.Systemroles.Select(role => new IdentityRole
                {
                    Name = role.ROLE_NAME,
                    Id = role.ROLE_ID.ToString()
                });
            }
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            context.Systemroles.Add(new Systemrole()
            {
                ROLE_NAME = role.Name,
                ROLE_DESC = role.Name                
            });

            var result = IdentityResult.Success;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                result = IdentityResult.Failed(new IdentityError { Description = $"Could not insert role {role.Name}." });
            }

            return result;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(role.Id);
            var customRole = await context.Systemroles.Where(r => r.ROLE_ID == id).FirstOrDefaultAsync();

            var result = IdentityResult.Success;

            if (customRole != null)
            {
                context.Systemroles.Remove(customRole);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    result = IdentityResult.Failed(new IdentityError { Description = $"Could not delete role {role.Name}." });
                }
            }

            return result;
        }

        public void Dispose()
        {
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(roleId);

            var role = await context.Systemroles.Where(r => r.ROLE_ID == id).FirstOrDefaultAsync();

            IdentityRole result = null;

            if (role != null)
            {
                result = new IdentityRole()
                {
                    Id = role.ROLE_ID.ToString(),
                    Name = role.ROLE_NAME
                };
            }

            return result;
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await context.Systemroles.Where(r => r.ROLE_NAME == normalizedRoleName).FirstOrDefaultAsync();

            IdentityRole result = null;

            if (role != null)
            {
                result = new IdentityRole()
                {
                    Id = role.ROLE_ID.ToString(),
                    Name = role.ROLE_NAME
                };
            }

            return result;
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    // User management - CRUD, password management and associating users with roles
    public class CustomUserStore : IQueryableUserStore<ApplicationUser>, 
        IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser> 
    {
        private readonly ClearConnectionContext context;
        private readonly ClearConnectionService connection;

        public CustomUserStore(ClearConnectionContext context, ClearConnectionService clearService)
        {
            this.context = context;
            this.connection = connection;
        }

        // Maps User instances (custom) to ApplicationUser instances (ASP.NET Core Identity)
        public IQueryable<ApplicationUser> Users
        {
            get
            {
                return this.context.People.Select(user => new ApplicationUser
                {
                    UserName = user.PERSONAL_EMAIL,
                    Email = user.PERSONAL_EMAIL,
                    Id = user.PERSON_ID.ToString()
                });
            }
        }

        // Adds user to a role by adding a new record in the UserRoles table
        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var roleId = await context.Systemroles.Where(r => r.ROLE_NAME == roleName).Select(r => r.ROLE_ID).FirstOrDefaultAsync();
            var userId = Convert.ToInt32(user.Id);

            context.PersonRoles.Add(new PersonRole()
            {
                PERSON_ID = userId,
                ROLE_ID = roleId
            });

            await context.SaveChangesAsync();
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {


            var customUser = new Person()
            {
                CREATED_DATE = DateTime.Now,
                UPDATED_DATE = DateTime.Now,
                IS_DELETED = false,
                FIRST_NAME = user.FIRST_NAME,
                MIDDLE_NAME = user.MIDDLE_NAME,
                LAST_NAME = user.LAST_NAME,
                COMPANY_NAME = user.COMPANY_NAME,
                PERSONALADDRESS1 = user.PERSONALADDRESS1,
                PERSONALADDRESS2 = user.PERSONALADDRESS2,
                PERSONAL_CITY = user.PERSONAL_CITY,
                PERSONAL_STATE_ID = user.PERSONAL_STATE_ID,
                PERSONAL_COUNTRY_ID = user.PERSONAL_COUNTRY_ID,
                PERSONAL_MOBILE = user.PERSONAL_MOBILE,
                PERSONAL_PHONE = user.PERSONAL_PHONE,
                BUSINESS_ADDRESS1 = user.BUSINESS_ADDRESS1,
                BUSINESS_ADDRESS2 = user.BUSINESS_ADDRESS2,
                BUSINESS_CITY = user.BUSINESS_CITY,
                BUSINESS_COUNTRY_ID = user.BUSINESS_COUNTRY_ID,
                BUSINESS_EMAIL = string.IsNullOrEmpty(user.BUSINESS_EMAIL) ? user.Email : user.BUSINESS_EMAIL,
                BUSINESS_MOBILE = user.BUSINESS_MOBILE,
                BUSINESS_PHONE = user.BUSINESS_PHONE,
                BUSINESS_STATE_ID = user.BUSINESS_STATE_ID,
                BUSINESS_WEB_ADD = user.BUSINESS_WEB_ADD,
                PERSONAL_EMAIL = user.Email,
                PASSWORDHASH = user.PasswordHash,
                APPLICENCEID = user.APPLICENCEID,
                PERSON_STATUS = user.PERSON_STATUS,
                CURRENCY_ID = user.DEFAULT_CURRENCY,
                CURRENT_BALANCE = user.CURRENT_BALANCE,
                COMPANYTYPE = user.COMPANYTYPE,
                PERSONAL_POSTCODE = user.PERSONAL_POSTCODE,
                BUSINESS_POSTCODE = user.BUSINESS_POSTCODE
        };

            context.People.Add(customUser);

            var result = IdentityResult.Success;

            try
            {
                await context.SaveChangesAsync();
                user.Id = customUser.PERSON_ID.ToString();
            }
            catch (Exception ex)
            {
                result = IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
            }

            return result;
        }

       
        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(user.Id);
            var customUser = await context.People.Include(u => u.PersonRoles).Where(r => r.PERSON_ID == id).FirstOrDefaultAsync();

            var result = IdentityResult.Success;

            if (customUser != null)
            {
                context.People.Remove(customUser);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    result = IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Email}." });
                }
            }

            return result;
        }

        public void Dispose()
        {
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(userId);

            var result = await FindByAsync(u => u.PERSON_ID == id);

            return result;
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var result = await FindByAsync(u => u.PERSONAL_EMAIL == normalizedUserName);

            return result;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            //var result = await FindByAsync(u => u.PERSONAL_EMAIL == normalizedUserName);
            var user = await context.People.Where(u=>u.PERSONAL_EMAIL == normalizedUserName).FirstOrDefaultAsync();

            ApplicationUser result = null;

            if (user != null)
            {
                result = new ApplicationUser
                {
                    Id = user.PERSON_ID.ToString(),
                    UserName = user.PERSONAL_EMAIL,
                    Email = user.PERSONAL_EMAIL,
                    PasswordHash = user.PASSWORDHASH,
                    CompanyName = user.COMPANY_NAME,
                    CountryID = user.PERSONAL_COUNTRY_ID,
                    StateID = user.PERSONAL_STATE_ID,
                    CompanyId = user.PARENT_PERSON_ID,
                    CompanyTypeID = user.COMPANYTYPE
                };
            }

            return result;
        }

        // Finds a User instance based on some condition (predicate)
        private async Task<ApplicationUser> FindByAsync(Expression<Func<Person, bool>> predicate)
        {
            var user = await context.People.Include(p=>p.PersonRoles).ThenInclude(r=>r.Role).Where(predicate).FirstOrDefaultAsync();

            ApplicationUser result = null;

            if (user != null)
            {
                result = new ApplicationUser
                {
                    Id = user.PERSON_ID.ToString(),
                    UserName = user.PERSONAL_EMAIL,
                    Email = user.PERSONAL_EMAIL,
                    PasswordHash = user.PASSWORDHASH,
                    CompanyId = user.PARENT_PERSON_ID,
                    CompanyTypeID = user.COMPANYTYPE,
                    FIRST_NAME = user.FIRST_NAME,
                    MIDDLE_NAME = user.MIDDLE_NAME,
                    LAST_NAME = user.LAST_NAME,
                    COMPANY_NAME = user.COMPANY_NAME,                    
                    PERSONALADDRESS1 = user.PERSONALADDRESS1,
                    PERSONALADDRESS2 = user.PERSONALADDRESS2,
                    PERSONAL_CITY = user.PERSONAL_CITY,
                    PERSONAL_STATE_ID = user.PERSONAL_STATE_ID,
                    PERSONAL_COUNTRY_ID = user.PERSONAL_COUNTRY_ID,
                    PERSONAL_MOBILE = user.PERSONAL_MOBILE,
                    PERSONAL_PHONE = user.PERSONAL_PHONE,
                    BUSINESS_ADDRESS1 = user.BUSINESS_ADDRESS1,
                    BUSINESS_ADDRESS2 = user.BUSINESS_ADDRESS2,
                    BUSINESS_CITY = user.BUSINESS_CITY,
                    BUSINESS_COUNTRY_ID = user.BUSINESS_COUNTRY_ID,
                    BUSINESS_EMAIL = string.IsNullOrEmpty(user.BUSINESS_EMAIL) ? user.PERSONAL_EMAIL : user.BUSINESS_EMAIL,
                    BUSINESS_MOBILE = user.BUSINESS_MOBILE,
                    BUSINESS_PHONE = user.BUSINESS_PHONE,
                    BUSINESS_STATE_ID = user.BUSINESS_STATE_ID,
                    BUSINESS_WEB_ADD = user.BUSINESS_WEB_ADD,
                    PERSONAL_EMAIL = user.PERSONAL_EMAIL,                    
                    APPLICENCEID = user.APPLICENCEID != null ? (int)user.APPLICENCEID : 0,    
                 
                    CURRENT_BALANCE = user.CURRENT_BALANCE,
                    PersonalState = user.State != null ? user.State.STATENAME: string.Empty,
                    BusinessState = user.State1 != null ? user.State1.STATENAME: string.Empty,
                    PersonalCountry = user.Country != null ? user.Country.COUNTRYNAME: string.Empty,
                    BusinessCountry = user.Country1 != null ? user.Country1.COUNTRYNAME: string.Empty
                };
            }

             
            if (user.PersonRoles != null)
            {
                foreach(var role in user.PersonRoles)
                {
                    if(role.Role != null)
                     result.RoleNames = new string[] { role.Role.ROLE_NAME };
                }
                
            }

            return result;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(user.Id);

            var roles = await context.People
                               .Include(u => u.PersonRoles)
                               .Where(u => u.PERSON_ID == userId)
                               .SelectMany(u => u.PersonRoles.Select(ur => ur.Role.ROLE_NAME))
                               .ToListAsync();

            return roles;
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Email);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!String.IsNullOrEmpty(user.PasswordHash));
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(user.Id);

            var result = await context.People
                               .Include(u => u.PersonRoles)
                               .Where(u => u.PERSON_ID == id)
                               .SelectMany(u => u.PersonRoles.Where(ur => ur.Role.ROLE_NAME == roleName))
                               .AnyAsync();

            return result;
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var roleId = await context.Systemroles.Where(r => r.ROLE_NAME == roleName)
                                      .Select(r => r.ROLE_ID)
                                      .FirstOrDefaultAsync();

            var userId = Convert.ToInt32(user.Id);

            var userRole = await context.PersonRoles.Where(ur => ur.ROLE_ID == roleId && ur.PERSON_ID == userId).FirstOrDefaultAsync();

            context.PersonRoles.Remove(userRole);

            await context.SaveChangesAsync();
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(user.Id);
            var customUser = await context.People.Where(r => r.PERSON_ID == id).FirstOrDefaultAsync();

            var result = IdentityResult.Success;

            if (customUser != null)
            {
                customUser.PERSONAL_EMAIL = user.Email;
                customUser.PASSWORDHASH = user.PasswordHash;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    result = IdentityResult.Failed(new IdentityError { Description = $"Could not update user {user.Email}." });
                }
            }

            return result;
        }
    }
}
