using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.AutoRest
{
    public static class AutoRestAliases
    {
        [CakePropertyAlias]
        public static AutoRestRunner AutoRest(this ICakeContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            return new AutoRestRunner(ctx.FileSystem, ctx.Environment, ctx.ProcessRunner, ctx.Tools);
        }
    }
}
