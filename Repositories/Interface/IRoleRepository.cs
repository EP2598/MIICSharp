using LinqImplementation.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqImplementation.Repositories.Interface
{
    interface IRoleRepository
    {
        List<Roles> ViewRoles();
    }
}
