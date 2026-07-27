using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface IDataSeeder
    {
        Task SeedAsync(CancellationToken ct = default);
        // make only one function for seed w bta5od CancellationToken 34an al function de htt3aml m3 Database fel Request and Response 
    }
}
