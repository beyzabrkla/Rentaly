using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class ProcessManager : GenericManager<Process>, IProcessService
    {
        public ProcessManager(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
