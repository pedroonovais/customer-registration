using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CustomerRegistration.Contracts.Customers
{
    public sealed record CreateCustomerRequest(string Name, string Email, string Occupation);
}
