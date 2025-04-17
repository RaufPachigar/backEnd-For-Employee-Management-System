using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPiSFl.Core.Entities;
using WebAPiSFl.Core.Entities.Employee;

namespace WebAPiSFl.Core.Entities
{
    public class BasicTableMaintanance 
    {

        public virtual long? CreatorUserId { get; set; } 
        public virtual DateTime CreationTime { get; set; } = DateTime.Now;
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual long? LastModifierUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual long? DeleterUserId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
