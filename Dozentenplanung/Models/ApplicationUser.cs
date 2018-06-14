using System;
using Microsoft.AspNetCore.Identity;
using Dozentenplanung.Data;
using System.Threading.Tasks;

namespace Dozentenplanung.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdministrator { get; set; }
        public bool CanWrite { get; set; }

        public void SetAdministratorBooleanInContext(bool aBoolean, ApplicationDbContext aDbContext) {
            if (aBoolean) {
                this.IsAdministrator = aBoolean;
            } else {
                this.IsAdministrator = this.IsTheOnlyAdministratorInContext(aDbContext);
            }
        }

        public string IsAdministratorString() {
            return this.StringForBoolean(this.IsAdministrator);
        }

        public string CanWriteString() {
            return this.StringForBoolean(this.CanWrite);
        }

        public async Task<bool> DeleteInMangerAndContext(UserManager<ApplicationUser> aUserManager, ApplicationDbContext aContext) {
            if (this.IsTheOnlyAdministratorInContext(aContext)) {
                return false;
            } else {
                await aUserManager.DeleteAsync(this);
                return true;
            }
        }

        private string StringForBoolean(bool aBoolean) {
            //Answer the string for the given boolean
            if (aBoolean) {
                return "Ja";
            } else {
                return "Nein";
            }
        }

        private bool IsTheOnlyAdministratorInContext(ApplicationDbContext aDbContext) {
            //Answer true if the receiver is the only user with admin privileges
            foreach (ApplicationUser eachUser in aDbContext.Users)
            {
                if (eachUser.IsAdministrator && eachUser != this) return false;
            }
            return true;
        }
    }
}
