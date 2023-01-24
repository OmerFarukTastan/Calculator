using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Operation
    {
        #region Public Properties

        //Left side of the operation
        public string LeftSide { get; set; }

        //Right side of the operation
        public string RightSide { get; set; }

        //Type of the operation to perform
        public OperationType OperationType { get; set; }

        public Operation InnerOperation { get; set; }

        #endregion

        #region Constructor
        public Operation()
        {
            //Create empty string instead of having nulls
            this.LeftSide = string.Empty;
            this.RightSide = string.Empty;
        }
        #endregion
    }
}
