using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        /// <summary>
        /// Initialize Database
        /// </summary>
        public void Initialize();
    }
}
