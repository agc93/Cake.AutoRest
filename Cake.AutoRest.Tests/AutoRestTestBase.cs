using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.AutoRest.Tests
{
    public class AutoRestTestBase
    {
        protected AutoRestFixture Fixture { get; set; }

        protected AutoRestTestBase(AutoRestFixture fixture = null)
        {
            Fixture = fixture ?? new AutoRestFixture();
        }
    }
}
