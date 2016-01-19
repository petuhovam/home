using System;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Services.Domain;
using VendingMachine.Domain.Models;

namespace VendingMachine.UI.AspNetMvc.Services
{    
    public class MvcDomainModelContext : IDomainModelContext
    {
        public MvcDomainModelContext()
        {
            User = new User();
        }

        public User User
        {
            get;
            private set;
        }

        [Import]
        public IVMService VM
        {
            get;
            private set;
        }

        public void CreateDefaults()
        {
            User.CreateDefaults();
            VM.CreateDefaults();
        }
    }

    public interface IDomainModelContext
    {
        User User { get; }

        IVMService VM { get; }
    }
}