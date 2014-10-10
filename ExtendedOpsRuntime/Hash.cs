using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedOpsRuntime
{
    using System.ComponentModel.Composition;

    [Export(typeof(SimpleCalculator3.IOperation))]
    [ExportMetadata("Symbol", '#')]
    public class Hash : SimpleCalculator3.IOperation
    {
        public int Operate(int left, int right)
        {
            return left + right;
        }
    }
}
